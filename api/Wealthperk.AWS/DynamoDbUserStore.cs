using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Identity;
using Wealthperk.Model;

namespace Wealthperk.AWS
{
    public class DynamoDbUserStore : Microsoft.AspNetCore.Identity.IUserPasswordStore<UserInfo>,
                                        Microsoft.AspNetCore.Identity.IRoleStore<UserIdentityRole>,
                                            Microsoft.AspNetCore.Identity.IUserEmailStore<UserInfo>
    {        
        Amazon.DynamoDBv2.IAmazonDynamoDB _dynamoDb;
        public DynamoDbUserStore(Amazon.DynamoDBv2.IAmazonDynamoDB dynamoDb){
            
            _dynamoDb = dynamoDb;
        }

        public async Task<IdentityResult> CreateAsync(UserInfo user, CancellationToken cancellationToken)
        {
            var request = new UpdateItemRequest
            {
                Key = new Dictionary<string, AttributeValue>()
            {
                    { "Id", new AttributeValue {
                      N = "-1"
                  } }
            },
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
            {
                {":incr",new AttributeValue {
                     N = "1"
                 }}
            },
            UpdateExpression = "SET UserCounter = UserCounter + :incr",
            TableName = "Users",
            ReturnValues = "UPDATED_NEW" // Give me all attributes of the updated item.
            };
            var response = await _dynamoDb.UpdateItemAsync(request);
            var userId = response.Attributes["UserCounter"].N;

            await _dynamoDb.PutItemAsync("Users", new Dictionary<string, AttributeValue>
            {
                { "Id", new AttributeValue{N = userId}},
                { "UserName", new AttributeValue(user.UserName)},
                { "Email", new AttributeValue(user.Email) },
                { "PasswordHash", new AttributeValue(user.PasswordHash) }
            },
             cancellationToken);

             user.Id = userId;

             return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(UserInfo user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<UserInfo> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var record = await _dynamoDb.GetItemAsync("Users", new Dictionary<string, AttributeValue>
            {
                {"Id", new AttributeValue{N=userId}}
            });

            return new UserInfo {
                Id = record.Item["Id"].N,
                UserName = record.Item["UserName"].S,
                Email = record.Item["Email"].S
            };
        }

        public async Task<UserInfo> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var result = await _dynamoDb.QueryAsync(FindByNameQuery(normalizedUserName), cancellationToken);
            var record = result.Items.FirstOrDefault();

            if (record == null)
                return null;

            return new UserInfo {
                Id = record["Id"].N,
                UserName = record["Email"].S,
                Email = record["Email"].S
            };
        }

        public async Task<string> GetNormalizedUserNameAsync(UserInfo user, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return user.Email?.ToLower();
        }

        public async Task<string> GetUserIdAsync(UserInfo user, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return user.Id?.ToString();
        }

        public async Task<string> GetUserNameAsync(UserInfo user, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return user.Email;
        }

        public async Task SetNormalizedUserNameAsync(UserInfo user, string normalizedName, CancellationToken cancellationToken)
        {
            await Task.Yield();
            user.Email = normalizedName;
        }

        public async Task SetUserNameAsync(UserInfo user, string userName, CancellationToken cancellationToken)
        {
            await Task.Yield();
            user.UserName = userName;
        }

        public Task<IdentityResult> UpdateAsync(UserInfo user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DynamoDbUserStore() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        #region private methods
        
        private QueryRequest FindByNameQuery(string normalizedUserName)
        {
            var request = new QueryRequest
            {
                TableName = "Users",
                IndexName= "Email-index",
                KeyConditionExpression = "Email = :v_Email",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":v_Email", new AttributeValue { S =  normalizedUserName }}}
            };

            return request;
        }
        #endregion

        #region password
        public async Task SetPasswordHashAsync(UserInfo user, string passwordHash, CancellationToken cancellationToken)
        {
            await Task.Yield();
            user.PasswordHash = passwordHash;            
           /* await _dynamoDb.UpdateItemAsync("Passwords", new Dictionary<string, AttributeValue>
            {
                {"UserId", new AttributeValue{N=user.Id.ToString()}}},
               new Dictionary<string, AttributeValueUpdate>() 
               { {"Hash", new AttributeValueUpdate {
                   Value = new AttributeValue(passwordHash)
               }}                
            });        */   
        }

        public async Task<string> GetPasswordHashAsync(UserInfo user, CancellationToken cancellationToken)
        {
            var record = await _dynamoDb.GetItemAsync("Users", new Dictionary<string, AttributeValue>
            {
                {"Id", new AttributeValue {N = user.Id.ToString()}}              
            });

            return record?.Item["PasswordHash"].S;
        }

        public async Task<bool> HasPasswordAsync(UserInfo user, CancellationToken cancellationToken)
        {
            var res = await _dynamoDb.QueryAsync(new QueryRequest
            {
                TableName = "Passwords",
                KeyConditionExpression = "UserId = :v_userId",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                       {":v_userId", new AttributeValue { N =  user.Id.ToString() }}},
                ProjectionExpression = ""
            });

            return res.ScannedCount > 0;
        }

        Task<IdentityResult> IRoleStore<UserIdentityRole>.CreateAsync(UserIdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IdentityResult> IRoleStore<UserIdentityRole>.UpdateAsync(UserIdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IdentityResult> IRoleStore<UserIdentityRole>.DeleteAsync(UserIdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<string> IRoleStore<UserIdentityRole>.GetRoleIdAsync(UserIdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<string> IRoleStore<UserIdentityRole>.GetRoleNameAsync(UserIdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IRoleStore<UserIdentityRole>.SetRoleNameAsync(UserIdentityRole role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<string> IRoleStore<UserIdentityRole>.GetNormalizedRoleNameAsync(UserIdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IRoleStore<UserIdentityRole>.SetNormalizedRoleNameAsync(UserIdentityRole role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<UserIdentityRole> IRoleStore<UserIdentityRole>.FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<UserIdentityRole> IRoleStore<UserIdentityRole>.FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

       async  Task IUserEmailStore<UserInfo>.SetEmailAsync(UserInfo user, string email, CancellationToken cancellationToken)
        {
            await Task.Yield();
            user.UserName = email;
        }

        async Task<string> IUserEmailStore<UserInfo>.GetEmailAsync(UserInfo user, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return user.UserName;
        }

        async Task<bool> IUserEmailStore<UserInfo>.GetEmailConfirmedAsync(UserInfo user, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return true;
        }

        async Task IUserEmailStore<UserInfo>.SetEmailConfirmedAsync(UserInfo user, bool confirmed, CancellationToken cancellationToken)
        {
            await Task.Yield();
        }

        Task<UserInfo> IUserEmailStore<UserInfo>.FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return FindByNameAsync(normalizedEmail, cancellationToken);
        }

        async Task<string> IUserEmailStore<UserInfo>.GetNormalizedEmailAsync(UserInfo user, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return user.Email;
        }

        async Task IUserEmailStore<UserInfo>.SetNormalizedEmailAsync(UserInfo user, string normalizedEmail, CancellationToken cancellationToken)
        {
            await Task.Yield();
            user.Email = normalizedEmail;
        }
        #endregion

    }
}
