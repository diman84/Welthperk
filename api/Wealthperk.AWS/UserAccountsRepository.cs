using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Wealthperk.AWS.Models;
using Wealthperk.Model;

namespace Wealthperk.AWS
{
    public class UserAccountsRepository : IUserAccountsRepository
    {
        Amazon.DynamoDBv2.IAmazonDynamoDB _dynamoDb;
        public UserAccountsRepository(Amazon.DynamoDBv2.IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        public async Task AddAccountsToUserAsync(object userId, IEnumerable<AccountInfo> accounts)
        {
            var item = await GetAccountsById(userId);

            await AddAccountsToUser(item, userId, accounts);
        }

        public async Task<IEnumerable<AccountInfo>> GetUserAccountsAsync(object userId)
        {
            var item = await GetAccountsById(userId);

            if (item == null)
                return Enumerable.Empty<AccountInfo>();

            return item.Select(UserModelFactory.CreateAccountFromAWS);
        }

        public async Task<IEnumerable<AccountInfo>> GetUserAccountsByUserNameAsync(string email)
        {
            return (await GetAccountsByEmailAsync(email))
            .Select(UserModelFactory.CreateAccountFromAWS).ToArray();;
        }

        private async Task<List<Document>> GetAccountsById(object userId)
        {
            var table = Table.LoadTable(_dynamoDb, "Users");
            var id = new Primitive(userId.ToString(), true);
            var item = await table.GetItemAsync(id, new GetItemOperationConfig
            {
                AttributesToGet = new List<string> {
                    "Accounts"
                }
            });
            if (item == null || !item.ContainsKey("Accounts"))
                return null;

            return item["Accounts"].AsListOfDocument();
        }
        private async Task<List<Document>> GetAccountsByEmailAsync(string email)
        {
            var table = Table.LoadTable(_dynamoDb, "Users");
            var search = table.Query(new QueryOperationConfig
            {
                IndexName = "Email-index",
                KeyExpression = new Expression
                {
                    ExpressionStatement = "Email = :v_Email",
                    ExpressionAttributeValues = new Dictionary<string, DynamoDBEntry>
                            {{":v_Email", email.ToUpper() }}
                }
            });

            var result = (await search.GetNextSetAsync()).First();
            if (!result.ContainsKey("Accounts"))
                return new List<Document>();

            return result["Accounts"].AsListOfDocument();
        }

         private async Task AddAccountsToUser(DynamoDBEntry item, object id, IEnumerable<AccountInfo> accounts)
        {
            var table = Table.LoadTable(_dynamoDb, "Users");
            var doc = new Document();
            var list = item != null
                         ? item.AsListOfDocument()
                         : new List<Document>();
            list.AddRange(accounts
                .Where(x => list.All(d => d["AccountId"].AsString() != x.AccountId))
                .Select(x => Document.FromAttributeMap(UserModelFactory.MapAccountInfoToAWS(x))));

            doc["Accounts"] = list;

            await table.UpdateItemAsync(doc, new Primitive(id.ToString(), true));
        }
    }
}