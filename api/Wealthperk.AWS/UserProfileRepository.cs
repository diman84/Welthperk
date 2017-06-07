using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using Wealthperk.AWS.Models;
using Wealthperk.Model;
using Wealthperk.Model.Profile;

namespace Wealthperk.AWS
{
    public class UserProfileRepository : IUserProfileRepository
    {
        Amazon.DynamoDBv2.IAmazonDynamoDB _dynamoDb;
        public UserProfileRepository(Amazon.DynamoDBv2.IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        public async Task SetProfileToUserAsync(object userId, UserProfile settings)
        {
            var item = await GetProfileById(userId);

            await SetUserProfileAsync(item, userId, settings);
        }

        public async Task<UserProfile> GetUserProfileAsync(object userId)
        {
            var item = await GetProfileById(userId);

            if (item == null)
                return null;

            return UserModelFactory.CreateUserProfileFromAWS(item);
        }

        public async Task<UserProfile> GetUserProfileByUserNameAsync(string email)
        {
            return UserModelFactory.CreateUserProfileFromAWS(await GetProfileByEmailAsync(email));
        }

        private async Task<Document> GetProfileById(object userId)
        {
            var table = Table.LoadTable(_dynamoDb, "Users");
            var id = new Primitive(userId.ToString(), true);
            var item = await table.GetItemAsync(id, new GetItemOperationConfig
            {
                AttributesToGet = new List<string> {
                    "Profile"
                }
            });
            if (item == null || !item.ContainsKey("Profile"))
                return null;

            return item["Profile"].AsDocument();
        }
        private async Task<Document> GetProfileByEmailAsync(string email)
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
            if (!result.ContainsKey("Profile"))
                return null;

            return result["Profile"].AsDocument();
        }

         private async Task SetUserProfileAsync(DynamoDBEntry item, object id, UserProfile settings)
        {
            var table = Table.LoadTable(_dynamoDb, "Users");
            var doc = new Document();
            doc["Profile"] = Document.FromJson(JsonConvert.SerializeObject(settings));

            await table.UpdateItemAsync(doc, new Primitive(id.ToString(), true));
        }
    }
}