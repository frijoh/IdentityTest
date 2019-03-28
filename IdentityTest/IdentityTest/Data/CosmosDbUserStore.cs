using IdentityTest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityTest.Data
{
    public class CosmosDbUserStore :
        IUserStore<ExtendedIdentityUser>,
        IUserEmailStore<ExtendedIdentityUser>,
        IUserPasswordStore<ExtendedIdentityUser>,
        IUserRoleStore<ExtendedIdentityUser>
    {
        private string EndpointUrl = "https://localhost:8081";
        private string AuthorizationKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private string DatabaseId = "IdentityTest";
        private string UserCollectionId = "IdentityUser";

        public Task AddToRoleAsync(ExtendedIdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (user.Roles == null)
                {
                    user.Roles = new List<string>();
                }
                user.Roles.Add(roleName);
            }, cancellationToken);
        }

        public Task<IdentityResult> CreateAsync(ExtendedIdentityUser user, CancellationToken cancellationToken)
        {
            var client = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey);
            return Task.Run(() =>
            {
                try
                {
                    //var result = await CreateDocument(user);
                    //user.DocumentId = "";
                    var result = client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, UserCollectionId), user);
                    return IdentityResult.Success;
                }
                catch (Exception)
                {
                    return IdentityResult.Failed(new IdentityError()
                    {
                        Code = "",
                        Description = "",
                    });
                }
            }, cancellationToken);
        }

        //private async Task<string> CreateDocument(ExtendedIdentityUser user)
        //{
        //    var client = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey);
        //    var result = await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, UserCollectionId), user);
        //    return result.Resource.Id;
        //}

        public Task<IdentityResult> DeleteAsync(ExtendedIdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Task<ExtendedIdentityUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            var client = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey);
            return Task.Run(() =>
            {
                var user = client.CreateDocumentQuery<ExtendedIdentityUser>(
                 UriFactory.CreateDocumentCollectionUri(DatabaseId, UserCollectionId))
                 .Where(identityuser => identityuser.NormalizedEmail == normalizedEmail)
                 .AsEnumerable()
                 .FirstOrDefault();
                return user;
            }, cancellationToken);
        }

        public Task<ExtendedIdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var client = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey);
            return Task.Run(() =>
            {
                var user = client.CreateDocumentQuery<ExtendedIdentityUser>(
                 UriFactory.CreateDocumentCollectionUri(DatabaseId, UserCollectionId))
                 .Where(identityuser => identityuser.Id == userId)
                 .AsEnumerable()
                 .FirstOrDefault();
                return user;
            }, cancellationToken);
        }

        public Task<ExtendedIdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var client = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey);
            return Task.Run(() =>
            {
                var user = client.CreateDocumentQuery<ExtendedIdentityUser>(
                 UriFactory.CreateDocumentCollectionUri(DatabaseId, UserCollectionId))
                 .Where(identityuser => identityuser.NormalizedUserName == normalizedUserName)
                 .AsEnumerable()
                 .FirstOrDefault();
                return user;
            }, cancellationToken);
        }

        public Task<string> GetEmailAsync(ExtendedIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                return user.Email;
            }, cancellationToken);
        }

        public Task<bool> GetEmailConfirmedAsync(ExtendedIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                return user.EmailConfirmed;
            }, cancellationToken);
        }

        public Task<string> GetNormalizedEmailAsync(ExtendedIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                return user.NormalizedEmail;
            }, cancellationToken);
        }

        public Task<string> GetNormalizedUserNameAsync(ExtendedIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                return user.NormalizedUserName;
            }, cancellationToken);
        }

        public Task<string> GetPasswordHashAsync(ExtendedIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                return user.PasswordHash;
            }, cancellationToken);
        }

        public Task<IList<string>> GetRolesAsync(ExtendedIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                return user.Roles;
            }, cancellationToken);
        }

        public Task<string> GetUserIdAsync(ExtendedIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                return user.Id;
            }, cancellationToken);
        }

        public Task<string> GetUserNameAsync(ExtendedIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                return user.UserName;
            }, cancellationToken);
        }

        public Task<IList<ExtendedIdentityUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(ExtendedIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                return string.IsNullOrEmpty(user.PasswordHash);
            }, cancellationToken);
        }

        public Task<bool> IsInRoleAsync(ExtendedIdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                return user.Roles == null ? false : user.Roles.Contains(roleName);
            }, cancellationToken);
        }

        public Task RemoveFromRoleAsync(ExtendedIdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(ExtendedIdentityUser user, string email, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                user.Email = email;
            }, cancellationToken);
        }

        public Task SetEmailConfirmedAsync(ExtendedIdentityUser user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                user.EmailConfirmed = confirmed;
            }, cancellationToken);
        }

        public Task SetNormalizedEmailAsync(ExtendedIdentityUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                user.NormalizedEmail = normalizedEmail;
            }, cancellationToken);
        }

        public Task SetNormalizedUserNameAsync(ExtendedIdentityUser user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                user.NormalizedUserName = normalizedName;
            }, cancellationToken);
        }

        public Task SetPasswordHashAsync(ExtendedIdentityUser user, string passwordHash, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                user.PasswordHash = passwordHash;
            }, cancellationToken);
        }

        public Task SetUserNameAsync(ExtendedIdentityUser user, string userName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                user.UserName = userName;
            }, cancellationToken);
        }

        public Task<IdentityResult> UpdateAsync(ExtendedIdentityUser user, CancellationToken cancellationToken)
        {
            var client = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey);
            return Task.Run(() =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(user.DocumentId))
                    {
                        var result = client.CreateDocumentQuery<ExtendedIdentityUser>(
                         UriFactory.CreateDocumentCollectionUri(DatabaseId, UserCollectionId))
                         .Where(identityuser => identityuser.Id == user.Id)
                         .AsEnumerable()
                         .FirstOrDefault();

                        user.DocumentId = result.DocumentId;
                        var updateResult = client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, UserCollectionId), user);
                    }
                    else
                    {
                        var result = client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, UserCollectionId), user);
                    }
                    return IdentityResult.Success;
                }
                catch (Exception)
                {
                    return IdentityResult.Failed(new IdentityError()
                    {
                        Code = "",
                        Description = "",
                    });
                }
            }, cancellationToken);
        }
    }
}
