using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Wealthperk.Model;
using Wealthperk.Model.Accounts;

namespace Wealthperk.AWS
{
    public class UserAccountTimeseriesRepository: IUserAccountTimeseriesRepository
    {
        Amazon.DynamoDBv2.IAmazonDynamoDB _dynamoDb;
        public UserAccountTimeseriesRepository(Amazon.DynamoDBv2.IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        public async Task<double?> GetLatestMarketValueForAccountAsync(string accountId)
        {
            var table = Table.LoadTable(_dynamoDb, "AccountBalances");
            var query = new QueryOperationConfig();
            query.BackwardSearch = true;
            query.Limit = 1;
            query.KeyExpression = new Expression();
            query.KeyExpression.ExpressionStatement = "AccountId = :v_accountId";
            query.KeyExpression.ExpressionAttributeValues[":v_accountId"] = accountId;

            var search = table.Query(query);
            var doc = (await search.GetNextSetAsync()).FirstOrDefault();
            if (doc == null || !doc.ContainsKey("MV"))
                return null;

            return doc["MV"].AsDouble() + (doc.ContainsKey("CF") && doc["CF"] != null ? doc["CF"].AsDouble() : 0);
        }

        public async Task<double?> GetStartMarketValueForAccountAsync(string accountId)
        {
            var table = Table.LoadTable(_dynamoDb, "AccountBalances");
            var query = new QueryOperationConfig();
            query.Limit = 1;
            query.KeyExpression = new Expression();
            query.KeyExpression.ExpressionStatement = "AccountId = :v_accountId";
            query.KeyExpression.ExpressionAttributeValues[":v_accountId"] = accountId;

            var search = table.Query(query);
            var doc = (await search.GetNextSetAsync()).FirstOrDefault();
            if (doc == null || !doc.ContainsKey("MV"))
                return null;

            return doc["MV"].AsDouble() + (doc.ContainsKey("CF") && doc["CF"] != null ? doc["CF"].AsDouble() : 0);
        }

        public async Task<double?> GetTotalCashFlowForAccountAsync(string accountId)
        {
            var table = Table.LoadTable(_dynamoDb, "AccountBalances");
            var query = new QueryOperationConfig();
            query.IndexName = "CFAccountId-Date-index";
            query.KeyExpression = new Expression();
            query.KeyExpression.ExpressionStatement = "CFAccountId = :v_accountId";
            query.KeyExpression.ExpressionAttributeValues[":v_accountId"] = accountId;
            query.AttributesToGet = new List<string> {"CF"};
            query.Select = SelectValues.SpecificAttributes;
            double? result = null;

            var search = table.Query(query);
            do
            {
                var list = await search.GetNextSetAsync();
                foreach (var doc in list){
                    if (doc != null && !doc.ContainsKey("CF") && doc["CF"] != null){
                        result = result.HasValue
                            ? result + doc["CF"].AsDouble()
                            : doc["CF"].AsDouble();
                    }
                }

            } while (!search.IsDone);
            return result;
        }

        public async Task UploadSeriesToAccountAsync(string accountId, IEnumerable<AccountTimeseriesValue> timeseries)
        {
            var table = Table.LoadTable(_dynamoDb, "AccountBalances");
            var batchWrite = table.CreateBatchWrite();

            foreach (var value in timeseries)
            {
                var seriesData = new Document();
                seriesData["AccountId"] = accountId;
                seriesData["Date"] = value.Date.ToAWSDate();
                seriesData["MV"] = value.MarketValue;
                if (value.CashFlow.HasValue){
                    seriesData["CF"] = value.CashFlow.Value;
                    seriesData["CFAccountId"] = accountId;
                }
                batchWrite.AddDocumentToPut(seriesData);
            }

            await batchWrite.ExecuteAsync();

        }
    }
}