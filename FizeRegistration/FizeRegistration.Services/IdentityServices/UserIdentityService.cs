using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FizeRegistration.Domain.Entities.Identity;
using FizeRegistration.Services.IdentityServices.Contracts;
using FizeRegistration.Common.Helpers;
using Microsoft.IdentityModel.Tokens;
using FizeRegistration.Common.Exceptions.IdentityExceptions;
using FizeRegistration.Common.IdentityConfiguration;
using FizeRegistration.Common;
using FizeRegistration.Domain.Repositories.Identity.Contracts;
using FizeRegistration.Domain.DbConnectionFactory;
using FizeRegistration.Domain.DbConnectionFactory.Contracts;
using FizeRegistration.Services.MailSenderServices.Contracts;
using FizeRegistration.Shared.DataContracts;
using Microsoft.AspNetCore.Http;

namespace FizeRegistration.Services.IdentityServices;

public class UserIdentityService : IUserIdentityService
{
    private readonly IIdentityRepositoriesFactory _identityRepositoriesFactory;
    private readonly IAgencyRepositoriesFactory _agencyRepositoriesFactory;

    private readonly IDbConnectionFactory _connectionFactory;

    private readonly IMailSenderFactory _mailSenderFactory;

    public UserIdentityService(
        IDbConnectionFactory connectionFactory,
        IIdentityRepositoriesFactory identityRepositoriesFactory,
        IMailSenderFactory mailSenderFactory,
        IAgencyRepositoriesFactory detailsRepositoriesFactory)
    {
        _identityRepositoriesFactory = identityRepositoriesFactory;
        _connectionFactory = connectionFactory;
        _mailSenderFactory = mailSenderFactory;
        _agencyRepositoriesFactory = detailsRepositoriesFactory;
    }

    public Task<UserAccount> SignInAsync(AuthenticationDataContract authenticateDataContract) =>
          Task.Run(() =>
          {
              if (!Validator.IsEmailValid(authenticateDataContract.Email))
                  UserExceptionCreator<InvalidIdentityException>.Create(
                      IdentityValidationMessages.EMAIL_INVALID,
                      SignInErrorResponseModel.New(SignInErrorResponseType.InvalidEmail,
                          IdentityValidationMessages.EMAIL_INVALID)).Throw();

              using (IDbConnection connection = _connectionFactory.NewSqlConnection())
              {
                  IIdentityRepository repository = _identityRepositoriesFactory.NewIdentityRepository(connection);
                  UserIdentity user = repository.GetUserByEmail(authenticateDataContract.Email);

                  if (user == null)
                  {
                      UserExceptionCreator<InvalidIdentityException>.Create(
                          IdentityValidationMessages.INVALID_CREDENTIALS,
                          SignInErrorResponseModel.New(SignInErrorResponseType.InvalidCredentials,
                              IdentityValidationMessages.INVALID_CREDENTIALS)).Throw();
                  }

                  if (user.IsDeleted)
                  {
                      UserExceptionCreator<InvalidIdentityException>.Create(
                          IdentityValidationMessages.USER_DELETED,
                          SignInErrorResponseModel.New(SignInErrorResponseType.UserDeleted,
                              IdentityValidationMessages.USER_DELETED)).Throw();
                  }

                  if (!CryptoHelper.Validate(authenticateDataContract.Password, user.PasswordSalt, user.PasswordHash))
                  {
                      UserExceptionCreator<InvalidIdentityException>.Create(
                          IdentityValidationMessages.INVALID_CREDENTIALS,
                          SignInErrorResponseModel.New(SignInErrorResponseType.InvalidCredentials,
                              IdentityValidationMessages.INVALID_CREDENTIALS)).Throw();
                  }

                  byte[] key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings.TokenSecret);
                  DateTime expiry = DateTime.UtcNow.AddDays(ConfigurationManager.AppSettings.TokenExpiryDays);
                  ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
                      {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Expiration, expiry.Ticks.ToString())
                      }
                  );

                  claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));

                  SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                  {
                      Issuer = AuthOptions.ISSUER,
                      Audience = AuthOptions.AUDIENCE_LOCAL,
                      Subject = claims,
                      Expires = expiry,
                      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                          SecurityAlgorithms.HmacSha256Signature)
                  };

                  JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                  JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

                  UserAccount userData = new UserAccount(user)
                  {
                      TokenExpiresAt = expiry,
                      Token = tokenHandler.WriteToken(token),
                  };

                  if (IsUserPasswordExpired(user))
                  {
                      repository.UpdateUserExperationDate(user.Id, true);
                      UserExceptionCreator<InvalidIdentityException>.Create(
                          IdentityValidationMessages.PASSWORD_EXPIRED,
                          SignInErrorResponseModel.New(SignInErrorResponseType.PasswordExpired,
                              userData.CanUserResetExpiredPassword
                                  ? IdentityValidationMessages.PASSWORD_EXPIRED
                                  : IdentityValidationMessages.PASSWORD_EXPIRED_PLEASE_RESET, userData)).Throw();
                  }
                  else
                  {
                      user.LastLoggedIn = DateTime.Now;
                      userData.LastLoggedIn = user.LastLoggedIn;

                      repository.UpdateUserLastLoggedInDate(user.Id, user.LastLoggedIn.Value);
                  }

                  return userData;
              }
          });

    public Task<UserAccount> ValidateToken(ClaimsPrincipal userPrincipal) =>
        Task.Run(() =>
        {
            long userId = long.Parse(userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
            string email = userPrincipal.FindFirstValue(ClaimTypes.Email);

            using (IDbConnection connection = _connectionFactory.NewSqlConnection())
            {
                IIdentityRepository repository = _identityRepositoriesFactory.NewIdentityRepository(connection);

                UserIdentity currentUser = repository.GetUserById(userId);

                if (currentUser == null)
                {
                    UserExceptionCreator<InvalidIdentityException>.Create(
                        IdentityValidationMessages.TOKEN_INVALID,
                        SignInErrorResponseModel.New(SignInErrorResponseType.InvalidToken,
                            IdentityValidationMessages.TOKEN_INVALID)).Throw();
                }

                if (currentUser.IsDeleted)
                {
                    UserExceptionCreator<InvalidIdentityException>.Create(
                        IdentityValidationMessages.USER_DELETED,
                        SignInErrorResponseModel.New(SignInErrorResponseType.UserDeleted,
                            IdentityValidationMessages.USER_DELETED)).Throw();
                }

                if (email == null || !email.Equals(currentUser.Email, StringComparison.OrdinalIgnoreCase))
                {
                    UserExceptionCreator<InvalidIdentityException>.Create(
                        IdentityValidationMessages.EMAIL_INVALID,
                        SignInErrorResponseModel.New(SignInErrorResponseType.InvalidEmail,
                            IdentityValidationMessages.EMAIL_INVALID)).Throw();
                }

                if (IsUserPasswordExpired(currentUser))
                {
                    UserExceptionCreator<InvalidIdentityException>.Create(
                        IdentityValidationMessages.PASSWORD_EXPIRED,
                        SignInErrorResponseModel.New(SignInErrorResponseType.PasswordExpired,
                            IdentityValidationMessages.PASSWORD_EXPIRED)).Throw();
                }

                DateTime tokenExpiresAt = new DateTime(long.Parse(userPrincipal.FindFirstValue(ClaimTypes.Expiration)));

                if ((tokenExpiresAt - DateTime.Now).TotalDays < 1.0)
                {
                    UserExceptionCreator<InvalidIdentityException>.Create(
                        IdentityValidationMessages.TOKEN_EXPIRED,
                        SignInErrorResponseModel.New(SignInErrorResponseType.TokenExpired,
                            IdentityValidationMessages.TOKEN_EXPIRED)).Throw();
                }

                currentUser.LastLoggedIn = DateTime.Now;

                repository.UpdateUserLastLoggedInDate(currentUser.Id, currentUser.LastLoggedIn.Value);

                return new UserAccount(currentUser)
                {
                    TokenExpiresAt = tokenExpiresAt,
                };
            }
        });

    public Task<UserAccount> NewUser(NewUserDataContract newUserDataContract) =>
    Task.Run(() =>
    {
        using (IDbConnection connection = _connectionFactory.NewSqlConnection())
        {
            IIdentityRepository identityRepository = _identityRepositoriesFactory.NewIdentityRepository(connection);

            if (!Regex.IsMatch(newUserDataContract.Password, ConfigurationManager.AppSettings.PasswordStrongRegex))
            {
                throw new ArgumentException(ConfigurationManager.AppSettings.PasswordWeakErrorMessage);
            }

            if (!Validator.IsEmailValid(newUserDataContract.Email))
            {
                throw new ArgumentException(IdentityValidationMessages.EMAIL_INVALID);
            }

            if (!identityRepository.IsEmailAvailable(newUserDataContract.Email))
            {
                throw new ArgumentException(IdentityValidationMessages.EMAIL_NOT_AVAILABLE);
            }

            string passwordSalt = CryptoHelper.CreateSalt();

            string hashedPassword = CryptoHelper.Hash(newUserDataContract.Password, passwordSalt);

            UserIdentity newUser = new UserIdentity
            {
                CanUserResetExpiredPassword = true,
                Email = newUserDataContract.Email,
                PasswordExpiresAt =
                    (newUserDataContract.PasswordExpiresAt.Date - DateTime.Now.Date).TotalDays < 0
                        ? DateTime.Now.Date.AddDays(ConfigurationManager.AppSettings.PasswordExpiryDays)
                        : newUserDataContract.PasswordExpiresAt,
                PasswordHash = hashedPassword,
                PasswordSalt = passwordSalt
            };

            newUser.IsPasswordExpired = (newUser.PasswordExpiresAt - DateTime.Now.Date).TotalDays < 0;

            newUser.Id = identityRepository.NewUser(newUser);

            return identityRepository.GetAccountByUserId(newUser.Id);
        }
    });

    public Task NewAgency(AgencyDataContract agencyDataContract) =>
     Task.Run(() =>
     {
         using (IDbConnection connection = _connectionFactory.NewSqlConnection())
         {
             IAgencyRepository AgencyRepository = _agencyRepositoriesFactory.NewAgencyRepository(connection);
             IIdentityRepository identityRepository = _identityRepositoriesFactory.NewIdentityRepository(connection);

             Agencion agency = new Agencion
             {

                 AgencyName = agencyDataContract.AgencyName,
                 FirstName = agencyDataContract.FirstName,
                 Color = agencyDataContract.Color,
                 LastName = agencyDataContract.LastName,
                 Link = agencyDataContract.Link,
                 PhoneNumber = agencyDataContract.PhoneNumber,
                 LinkLogo = agencyDataContract.LinkLogo,
                 LinkPictureUser = agencyDataContract.LinkPicture,
                 WebSite = agencyDataContract.WebSite,
             };
             var User = identityRepository.GetUserByEmail(agencyDataContract.Email);
             long IdAgency = AgencyRepository.AddAgency(agency);
             AgencyRepository.UpdateAgencyId(IdAgency, User.Id);
         }
     });
    public Task IssueConfirmation(UserEmailDataContract userEmailDataContract, string baseUrl) =>
    Task.Run(() =>
    {
        using (IDbConnection connection = _connectionFactory.NewSqlConnection())
        {
            IIdentityRepository identityRepository = _identityRepositoriesFactory.NewIdentityRepository(connection);

            if (!Validator.IsEmailValid(userEmailDataContract.Email))
            {
                throw new ArgumentException(IdentityValidationMessages.EMAIL_INVALID);

            }

            if (!identityRepository.IsEmailAvailable(userEmailDataContract.Email))
            {
                throw new ArgumentException(IdentityValidationMessages.EMAIL_NOT_AVAILABLE);
            }

            byte[] key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings.TokenSecret);
            DateTime expiry = DateTime.UtcNow.AddMinutes(30);

            ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
                {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(ClaimTypes.Expiration, expiry.Ticks.ToString()),
                            new Claim(ClaimTypes.Email, userEmailDataContract.Email),
                            new Claim(ClaimTypes.Role, "UnconfirmedUser")
                }
            );

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = AuthOptions.ISSUER,
                Audience = AuthOptions.AUDIENCE_LOCAL,
                Subject = claims,
                Expires = expiry,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            tokenHandler.OutboundClaimTypeMap.Clear();

            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            TokenDataContract tokenData = new TokenDataContract
            {
                Token = tokenHandler.WriteToken(token),
            };

            var mailSenderService = _mailSenderFactory.NewMailSenderService();

            mailSenderService.SendTokenToEmail(userEmailDataContract.Email,
            tokenData, baseUrl);
        }
    });

    private bool IsUserPasswordExpired(
        UserIdentity user)
    {
        if (user.IsPasswordExpired) { return true; }

        if (DateTime.UtcNow > user.PasswordExpiresAt)
        {
            user.IsPasswordExpired = true;
            return true;
        }

        return false;
    }
}