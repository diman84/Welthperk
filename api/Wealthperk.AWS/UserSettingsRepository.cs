using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Wealthperk.AWS.Models;
using Wealthperk.Model;

namespace Wealthperk.AWS
{
    public class UserSettingsRepository : IUserSettingsRepository
    {
        Amazon.DynamoDBv2.IAmazonDynamoDB _dynamoDb;
        public UserSettingsRepository(Amazon.DynamoDBv2.IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        public async Task SetSettingsToUserAsync(object userId, PortfolioStrategy settings)
        {
            var item = await GetSettingsById(userId);

            await SetUserSettingsAsync(item, userId, settings);
        }

        public async Task<PortfolioStrategy> GetUserSettingsAsync(object userId)
        {
            var item = await GetSettingsById(userId);

            if (item == null)
                return null;

            return UserModelFactory.CreateUserSettingsFromAWS(item);
        }

        public async Task<PortfolioStrategy> GetUserSettingsByUserNameAsync(string email)
        {
            return UserModelFactory.CreateUserSettingsFromAWS(await GetSettingsByEmailAsync(email));
        }

        private async Task<Document> GetSettingsById(object userId)
        {
            var table = Table.LoadTable(_dynamoDb, "Users");
            var id = new Primitive(userId.ToString(), true);
            var item = await table.GetItemAsync(id, new GetItemOperationConfig
            {
                AttributesToGet = new List<string> {
                    "Settings"
                }
            });
            if (item == null || !item.ContainsKey("Settings"))
                return null;

            return item["Settings"].AsDocument();
        }
        private async Task<Document> GetSettingsByEmailAsync(string email)
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
            if (!result.ContainsKey("Settings"))
                return null;

            return result["Settings"].AsDocument();
        }

         private async Task SetUserSettingsAsync(DynamoDBEntry item, object id, PortfolioStrategy settings)
        {
            var table = Table.LoadTable(_dynamoDb, "Users");
            var doc = item != null
                         ? item.AsDocument()
                         : new Document();

           UserModelFactory.MapPortfolioSettingsToAWS(doc);

            doc["Settings"] = item;

            await table.UpdateItemAsync(doc, new Primitive(id.ToString(), true));
        }
    }
}