using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using OpenIddict.Core;
using Wealthperk.AWS.Models;

namespace Wealthperk.AWS
{
    public class OpenIddictTokenStore: IOpenIddictTokenStore<OpenIddictToken>        
    {
         Amazon.DynamoDBv2.IAmazonDynamoDB _dynamoDb;
         const string TableName = "OpenIdTokens";

        public OpenIddictTokenStore( Amazon.DynamoDBv2.IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        /// <summary>
        /// Creates a new token.
        /// </summary>
        /// <param name="token">The token to create.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation, whose result returns the token.
        /// </returns>
        public virtual async Task<OpenIddictToken> CreateAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (String.IsNullOrWhiteSpace(token.Id)){
                token.Id = Guid.NewGuid().ToString("N");
            }

            await _dynamoDb.PutItemAsync(TableName,new Dictionary<string, AttributeValue> {
                    {"Id", new AttributeValue { S =  token.Id }},
                    {"Subject", new AttributeValue { S =  token.Subject }},
                    {"Type", new AttributeValue { S =  token.Type }},
                    {"ApplicationId", new AttributeValue { S =  token.ApplicationId }},
                    {"AuthorizationId", new AttributeValue { S =  token.AuthorizationId }}});

            return token;
        }

        /// <summary>
        /// Creates a new token, which is associated with a particular subject.
        /// </summary>
        /// <param name="type">The token type.</param>
        /// <param name="subject">The subject associated with the token.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation, whose result returns the token.
        /// </returns>
        public virtual Task<OpenIddictToken> CreateAsync( string type,  string subject, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentException("The token type cannot be null or empty.");
            }

            return CreateAsync(new OpenIddictToken { Subject = subject, Type = type }, cancellationToken);
        }

        /// <summary>
        /// Retrieves an token using its unique identifier.
        /// </summary>
        /// <param name="identifier">The unique identifier associated with the token.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns the token corresponding to the unique identifier.
        /// </returns>
        public virtual async Task<OpenIddictToken> FindByIdAsync(string identifier, CancellationToken cancellationToken)
        {
            var key = ConvertIdentifierFromString(identifier);

            var result = await _dynamoDb.QueryAsync(new QueryRequest
            {
                TableName = "OpenIdAuthorizations",
                KeyConditionExpression = "Id = :v_Id",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":v_Id", new AttributeValue { S =  key }}},
            }, cancellationToken);

            var record = result.Items.FirstOrDefault();
            
            return OpenIdModelFactory.CreateTokenFromAWS(record);
        }

        /// <summary>
        /// Retrieves the list of tokens corresponding to the specified subject.
        /// </summary>
        /// <param name="subject">The subject associated with the tokens.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns the tokens corresponding to the specified subject.
        /// </returns>
        public virtual async Task<OpenIddictToken[]> FindBySubjectAsync(string subject, CancellationToken cancellationToken)
        {
            var request = new QueryRequest
            {
                TableName = TableName,
                IndexName= "Subject-index",
                KeyConditionExpression = "Subject = :v_Subject",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":v_Subject", new AttributeValue { S =  subject }}}
            };

            var result = await _dynamoDb.QueryAsync(request, cancellationToken);

            return result.Items.Select(AWS.Models.OpenIdModelFactory.CreateTokenFromAWS).ToArray();
        }

        /// <summary>
        /// Retrieves the unique identifier associated with a token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns the unique identifier associated with the token.
        /// </returns>
        public virtual Task<string> GetIdAsync( OpenIddictToken token, CancellationToken cancellationToken)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return Task.FromResult(ConvertIdentifierToString(token.Id));
        }

        /// <summary>
        /// Retrieves the token type associated with a token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns the token type associated with the specified token.
        /// </returns>
        public virtual Task<string> GetTokenTypeAsync( OpenIddictToken token, CancellationToken cancellationToken)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return Task.FromResult(token.Type);
        }

        /// <summary>
        /// Retrieves the subject associated with a token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns the subject associated with the specified token.
        /// </returns>
        public virtual Task<string> GetSubjectAsync( OpenIddictToken token, CancellationToken cancellationToken)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return Task.FromResult(token.Subject);
        }

        /// <summary>
        /// Revokes a token.
        /// </summary>
        /// <param name="token">The token to revoke.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>A <see cref="Task"/> that can be used to monitor the asynchronous operation.</returns>
        public virtual async Task RevokeAsync( OpenIddictToken token, CancellationToken cancellationToken)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            await _dynamoDb.DeleteItemAsync(TableName, new Dictionary<string, AttributeValue> {
                {"Id", new AttributeValue(token.Id)}
            }
            ,cancellationToken);
        }

        /// <summary>
        /// Sets the authorization associated with a token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="identifier">The unique identifier associated with the authorization.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation.
        /// </returns>
        public virtual async Task SetAuthorizationAsync(OpenIddictToken token,string identifier, CancellationToken cancellationToken)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }
            //TODO: check if it is nesessary to have the same logic as in RDS

            /*
            if (!string.IsNullOrEmpty(identifier))
            {
                var key = ConvertIdentifierFromString(identifier);                               
                var authorization = await Authorizations.SingleOrDefaultAsync(element => element.Id.Equals(key));
                if (authorization == null)
                {
                    throw new InvalidOperationException("The authorization associated with the token cannot be found.");
                }
            }

            else
            {
                var key = await GetIdAsync(token, cancellationToken);

                // Try to retrieve the authorization associated with the token.
                // If none can be found, assume that no authorization is attached.
                var authorization = await Authorizations.SingleOrDefaultAsync(element => element.Tokens.Any(t => t.Id.Equals(key)));
                if (authorization != null)
                {
                    authorization.Tokens.Remove(token);
                }
            }*/
            await Task.Yield();
            var key = ConvertIdentifierFromString(identifier);               
            token.AuthorizationId = key;
        }

        /// <summary>
        /// Sets the client application associated with a token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="identifier">The unique identifier associated with the client application.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation.
        /// </returns>
        public virtual async Task SetClientAsync( OpenIddictToken token, string identifier, CancellationToken cancellationToken)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }
            //TODO: check if it is nesessary to have the same logic as in RDS

           /* if (!string.IsNullOrEmpty(identifier))
            {
                var key = ConvertIdentifierFromString(identifier);

                var application = await Applications.SingleOrDefaultAsync(element => element.Id.Equals(key));
                if (application == null)
                {
                    throw new InvalidOperationException("The application associated with the token cannot be found.");
                }

                application.Tokens.Add(token);
            }

            else
            {
                var key = await GetIdAsync(token, cancellationToken);

                // Try to retrieve the application associated with the token.
                // If none can be found, assume that no application is attached.
                var application = await Applications.SingleOrDefaultAsync(element => element.Tokens.Any(t => t.Id.Equals(key)));
                if (application != null)
                {
                    application.Tokens.Remove(token);
                }
            }*/
            await Task.Yield();
            var key = ConvertIdentifierFromString(identifier);
            token.ApplicationId = key;
        }

        /// <summary>
        /// Updates an existing token.
        /// </summary>
        /// <param name="token">The token to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation.
        /// </returns>
        public virtual async Task UpdateAsync( OpenIddictToken token, CancellationToken cancellationToken)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            await _dynamoDb.UpdateItemAsync(TableName, new Dictionary<string, AttributeValue>{
                {"Id", new AttributeValue(token.Id)},               
            }, new Dictionary<string, AttributeValueUpdate>{
                {"ApplicationId", new AttributeValueUpdate(new AttributeValue(token.ApplicationId),
                    AttributeAction.PUT)},
                {"AuthorizationId",  new AttributeValueUpdate(new AttributeValue(token.AuthorizationId),
                    AttributeAction.PUT)},
                {"Subject",  new AttributeValueUpdate(new AttributeValue(token.Subject),
                    AttributeAction.PUT)},
                {"Type", new AttributeValueUpdate(new AttributeValue(token.Type),
                    AttributeAction.PUT)}
            });
            
        }

        /// <summary>
        /// Converts the provided identifier to a strongly typed key object.
        /// </summary>
        /// <param name="identifier">The identifier to convert.</param>
        /// <returns>An instance of <typeparamref name="string"/> representing the provided identifier.</returns>
        public virtual string ConvertIdentifierFromString(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                return default(string);
            }

            return (string) TypeDescriptor.GetConverter(typeof(string))
                                        .ConvertFromInvariantString(identifier);
        }

        /// <summary>
        /// Converts the provided identifier to its string representation.
        /// </summary>
        /// <param name="identifier">The identifier to convert.</param>
        /// <returns>A <see cref="string"/> representation of the provided identifier.</returns>
        public virtual string ConvertIdentifierToString(string identifier)
        {
            if (Equals(identifier, default(string)))
            {
                return null;
            }

            return TypeDescriptor.GetConverter(typeof(string))
                                 .ConvertToInvariantString(identifier);
        }
    }
}