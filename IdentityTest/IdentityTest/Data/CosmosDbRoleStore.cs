using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Documents.Client;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityTest.Data
{
    public class CosmosDbRoleStore : IRoleStore<IdentityRole>
    {
        private string EndpointUrl = "https://localhost:8081";
        private string AuthorizationKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private string DatabaseId = "IdentityTest";
        private string RoleCollectionId = "IdentityRole";

        public Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            var client = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey);
            return Task.Run(() =>
            {
                try
                {
                    var result = client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, RoleCollectionId), role);
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

        public Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var client = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey);
            return Task.Run(() =>
            {
                return client.CreateDocumentQuery<IdentityRole>(
                 UriFactory.CreateDocumentCollectionUri(DatabaseId, RoleCollectionId))
                 .Where(identityRole => identityRole.NormalizedName == normalizedRoleName)
                 .AsEnumerable()
                 .FirstOrDefault();
            }, cancellationToken);
        }

        public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                return role.Name;
            }, cancellationToken);
        }

        public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                role.NormalizedName = normalizedName;
            }, cancellationToken);
        }

        public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
