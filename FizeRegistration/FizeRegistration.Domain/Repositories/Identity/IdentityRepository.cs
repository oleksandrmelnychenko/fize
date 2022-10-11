using System;
using System.Data;
using System.Linq;
using FizeRegistration.Domain.Repositories.Identity.Contracts;
using Dapper;
using FizeRegistration.Domain.Entities.Identity;
using FizeRegistration.Shared.DataContracts;

namespace FizeRegistration.Domain.Repositories.Identity;

public class IdentityRepository : IIdentityRepository
{
    private readonly IDbConnection _connection;

    public IdentityRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public void RestoreUser(long id) =>
        _connection.Execute(
            "UPDATE [UserIdentities] " +
            "SET IsDeleted = 0 " +
            "WHERE [UserIdentities].Id = @Id",
            new { Id = id }
        );

    public int UpdateUserPassword(UserIdentity user) =>
        _connection.Execute(
            "UPDATE UserIdentities Set " +
            "PasswordExpiresAt = @PasswordExpiresAt, IsPasswordExpired = @IsPasswordExpired, PasswordSalt = @PasswordSalt, PasswordHash = @PasswordHash, LastModified = getutcdate() " +
            "WHERE Id = @Id",
            new
            {
                Id = user.Id,
                PasswordExpiresAt = user.PasswordExpiresAt,
                IsPasswordExpired = user.IsPasswordExpired,
                PasswordSalt = user.PasswordSalt,
                PasswordHash = user.PasswordHash
            });

    public void UpdateUserLastLoggedInDate(long userId, DateTime current)
    {
        _connection.Execute(
            "UPDATE [UserIdentities] " +
            "SET LastLoggedIn = @Current " +
            "WHERE Id = @UserId",
            new { UserId = userId, Current = current }
        );
    }

    public bool IsEmailAvailable(string email)
    {
        return _connection.Query<bool>(
            "SELECT IIF(COUNT(1) > 0, 0, 1) " +
            "FROM [UserIdentities] " +
            "WHERE [UserIdentities].IsDeleted = 0 " +
            "AND [UserIdentities].Email = @Email",
            new { Email = email }
        ).Single();
    }

    public bool IsEmailExistsAndConfirmed(string email)
    {
        return _connection.Query<bool>(
            "SELECT IIF(COUNT(1) > 0, 1, 0) " +
            "FROM [UserIdentities] " +
            "WHERE [UserIdentities].IsDeleted = 0 " +
            "AND [UserIdentities].IsEmailConfirmed = 1" +
            "AND [UserIdentities].Email = @Email",
            new { Email = email }
        ).Single();
    }

    public UserIdentity GetUserByEmail(string email)
    {
        UserIdentity userIdentity = _connection.Query<UserIdentity>(
           "SELECT * " +
           "FROM [UserIdentities] " +
           "WHERE Email = @Email",
           new { Email = email }
       ).FirstOrDefault();

        return userIdentity;
    }

    public UserIdentity GetUserById(long userId) =>
        _connection.Query<UserIdentity>(
            "SELECT * FROM UserIdentities " +
            "WHERE Id = @Id",
            new { Id = userId }).SingleOrDefault();

    public UserIdentity NewUser(string name, string description, string email, string passwordHash, string passwordSalt) =>
        _connection.Query<UserIdentity>(
            "INSERT INTO [UserIdentities] (IsDeleted,IsPasswordExpired,CanUserResetExpiredPassword,Email,PasswordHash,PasswordSalt,PasswordExpiresAt) " +
            "VALUES(0,0,0,@Email,@PasswordHash,@PasswordSalt,@PasswordExpiresAt) " +
            "SELECT * FROM [UserIdentities] WHERE ID = (SELECT SCOPE_IDENTITY()) ",
            new
            {
                Name = name,
                Description = description,
                Email = email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PasswordExpiresAt = DateTime.Now.AddDays(1)
            }
        ).SingleOrDefault();

    public UserAccount GetAccountByUserId(long userId)
    {
        return  new UserAccount(_connection.Query<UserIdentity>(
            "SELECT [UserIdentities].* " +
            "FROM [UserIdentities] " +
            "WHERE [UserIdentities].Id = @UserId",
            new { UserId = userId }).FirstOrDefault());

        
    }

    public long NewUser(UserIdentity userIdentity) =>
        _connection.Query<long>(
            "INSERT INTO [UserIdentities] " +
            "(IsDeleted, IsPasswordExpired, CanUserResetExpiredPassword, Email, PasswordHash, PasswordSalt, PasswordExpiresAt, ForceChangePassword) " +
            "VALUES " +
            "(0, 0, @CanUserResetExpiredPassword, @Email, @PasswordHash, @PasswordSalt, @PasswordExpiresAt, @ForceChangePassword); " +
            "SELECT SCOPE_IDENTITY()",
            userIdentity
        ).Single();

    public void UpdateUserExperationDate(long userId, bool isExpired) =>
        _connection.Execute(
            "UPDATE UserIdentities Set IsPasswordExpired = @IsExpired, LastModified = getutcdate() " +
            "WHERE Id = @Id",
            new { Id = userId, IsExpired = isExpired }
        );
}
public class AgencyRepository : IAgencyRepository
{

    private readonly IDbConnection _connection;

    public AgencyRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public long AddAgency(Agencion agency) =>
        _connection.Query<long>("INSERT INTO [Agencion] " +
             "(Color,AgencyName,LinkLogo,LinkPictureUser,WebSite,LastName,Link,FirstName,PhoneNumber) " +
             "VALUES " +
             "(@Color,@AgencyName,@LinkLogo,@LinkPictureUser,@WebSite,@LastName,@Link,@FirstName,@PhoneNumber) " +
             "SELECT SCOPE_IDENTITY()",
             new
             {
                 Color = agency.Color,
                 AgencyName = agency.AgencyName,
                 WebSite = agency.WebSite,
                 LinkLogo = agency.LinkLogo,
                 LinkPictureUser = agency.LinkPictureUser,
                 LastName = agency.LastName,
                 Link = agency.Link,
                 FirstName = agency.FirstName,
                 PhoneNumber = agency.PhoneNumber,
             }).Single();

    public void ChangeAgency(Agencion agency) =>
        _connection.Execute("UPDATE [Agencion] " +
            "Set Color =@Color,AgencyName=@AgencyName,LinkLogo=@LinkLogo,LinkPictureUser = @LinkPictureUser,WebSite=@WebSite,LastName=@LastName,Link=@Link,FirstName=@FirstName,PhoneNumber =@PhoneNumber " +
             "WHERE Id = @Id",
             new
             {
                 Id = agency.Id,
                 Color = agency.Color,
                 AgencyName = agency.AgencyName,
                 WebSite = agency.WebSite,
                 LinkLogo = agency.LinkLogo,
                 LinkPictureUser = agency.LinkPictureUser,
                 LastName = agency.LastName,
                 Link = agency.Link,
                 FirstName = agency.FirstName,
                 PhoneNumber = agency.PhoneNumber,
             });

    public void UpdateAgencyId(long agencionId, long userIdentitiesId)
    {
        _connection.Execute(
            "UPDATE Agencion SET UserIdentityId = @UserIdentityId " +
            "WHERE Id = @Id",
            new { UserIdentityId = userIdentitiesId, Id = agencionId });
    }

    public void ChangeValueTable(string id, string columnName, string value)
    {
        _connection.Execute(
             "UPDATE [Agencion] " +
            $"Set {columnName} =@Value "+
             "WHERE Id = @Id",
              new { Value = value, Id = id });
    }

    public List<AgencyDataContract> GetAgency() =>
      _connection.Query<AgencyDataContract>("SELECT" +
            " * FROM [dbo].[Agencion]").ToList();
     
    public AgencyDataContract GetAgencyByID(string id)
    {
        return _connection.Query<AgencyDataContract>(
              "SELECT * FROM Agencion " +
              "WHERE Id = @Id",
              new { Id = id }).SingleOrDefault();
    }

    public List<AgencyDataContract> DeleteAgency(string id)
    {
        _connection.Execute("DELETE FROM Agencion " +
            "WHERE Id = @Id ",
            new {Id = id});
        return GetAgency(); 
    }
    public List<AgencyDataContract> FilterAgency(TableFilterContract filterParameter)
        => _connection.Query<AgencyDataContract>(
              "SELECT * FROM [Agencion] " +
          $"WHERE PATINDEX(N'%' + @Value + N'%', PhoneNumber) <> 0 " +
            $"OR PATINDEX(N'%' + @Value + N'%', FirstName) <> 0"+
            $"OR PATINDEX(N'%' + @Value + N'%', Link) <> 0" +
            $"OR PATINDEX(N'%' + @Value + N'%', LastName) <> 0" +
            $"OR PATINDEX(N'%' + @Value + N'%', WebSite) <> 0" +
            $"OR PATINDEX(N'%' + @Value + N'%', LinkPictureUser) <> 0" +
            $"OR PATINDEX(N'%' + @Value + N'%', LinkLogo) <> 0"+
            $"OR PATINDEX(N'%' + @Value + N'%', AgencyName) <> 0"+
            $"OR PATINDEX(N'%' + @Value + N'%', Color) <> 0",
              new { Value = filterParameter.ImputText }).ToList();
}


