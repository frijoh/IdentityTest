using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace IdentityTest.Data
{
    public class DocumentDBConfig
    {
        public static async void Configure()
        {
            var EndpointUrl = "https://localhost:8081";
            var AuthorizationKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
            var DatabaseId = "IdentityTest";
            var CollectionId = "Identity";

            var client = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey);
            var database = client.CreateDatabaseQuery().Where(db => db.Id == DatabaseId).AsEnumerable().FirstOrDefault();
            if (database == null)
            {
                database = await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
            }

            var col = client.CreateDocumentCollectionQuery(database.SelfLink).Where(c => c.Id == CollectionId).AsEnumerable().FirstOrDefault();
            if (col == null)
            {
                var collection = new DocumentCollection { Id = CollectionId };
                await client.CreateDocumentCollectionAsync(database.SelfLink, collection);
            }
        }
    }
}
