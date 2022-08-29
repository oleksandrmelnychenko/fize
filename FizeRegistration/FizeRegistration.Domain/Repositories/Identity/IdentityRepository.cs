﻿using System;
using System.Data;
using System.Linq;
using FizeRegistration.Domain.Repositories.Identity.Contracts;
using Dapper;
using FizeRegistration.Domain.Entities.Identity;

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
        UserAccount userAccount = new UserAccount(_connection.Query<UserIdentity>(
            "SELECT [UserIdentities].* " +
            "FROM [UserIdentities] " +
            "WHERE [UserIdentities].Id = @UserId",
            new { UserId = userId }).FirstOrDefault());

        return userAccount;
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
public class DetailsRepository : IDetailsRepository
{

    private readonly IDbConnection _connection;

    public DetailsRepository(IDbConnection connection)
    {
        _connection = connection;
    }
    public long NewDetails(Agencion details)
    {

        return _connection.Query<long>("INSERT INTO [Agencion] " +
             "(Color,AgencyName,LinkLogo,LinkPictureUser,WebSite,LastName,Link,FirstName,PhoneNumber) " +
             "VALUES " +
             "(@Color,@AgencyName,@LinkLogo,@LinkPictureUser,@WebSite,@LastName,@Link,@FirstName,@PhoneNumber) " +
             "SELECT SCOPE_IDENTITY()",
             new
             {
                 Color = details.Color,
                 AgencyName = details.AgencyName,
                 WebSite = details.WebSite,
                 LinkLogo = details.LinkLogo,
                 LinkPictureUser = details.LinkPictureUser,
                 LastName = details.LastName,
                 Link = details.Link,
                 FirstName = details.FirstName,
                 PhoneNumber = details.PhoneNumber,

             }).Single();
    }
    public void UpdateDetailsId(long AgencionId, long UserIdentitiesId)
    {
        _connection.Execute(
            "UPDATE Agencion SET UserIdentityId = @DetailsId " +
            "WHERE Id = @Id",
            new { DetailsId = UserIdentitiesId, Id = AgencionId });
    }
}

