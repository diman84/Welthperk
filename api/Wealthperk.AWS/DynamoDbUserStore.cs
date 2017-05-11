using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Identity;
using Wealthperk.Model;

namespace Wealthperk.AWS
{
    public class DynamoDbUserStore : Microsoft.AspNetCore.Identity.IUserStore<UserInfo>
    {        
        Amazon.DynamoDBv2.IAmazonDynamoDB _dynamoDb;
        public DynamoDbUserStore(Amazon.DynamoDBv2.IAmazonDynamoDB dynamoDb){
            
            _dynamoDb = dynamoDb;
        }

        public async Task<IdentityResult> CreateAsync(UserInfo user, CancellationToken cancellationToken)
        {
            await _dynamoDb.PutItemAsync("Users", new Dictionary<string, AttributeValue>
            {
                { "Name", new AttributeValue(user.Email)},
                { "Email", new AttributeValue(user.Email) }
            },
             cancellationToken);

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
                {"Id", new AttributeValue(userId)}                
            });

            return new UserInfo {
                Id = record.Item["Id"].N,
                UserName = record.Item["Email"].S,
                Email = record.Item["Email"].S
            };
        }

        public Task<UserInfo> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(UserInfo user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(UserInfo user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserNameAsync(UserInfo user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(UserInfo user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(UserInfo user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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

    }
}
