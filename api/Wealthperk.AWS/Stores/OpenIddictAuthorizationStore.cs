using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using OpenIddict.Core;
using Wealthperk.AWS.Models;

namespace Wealthperk.AWS
{
    public class OpenIddictAuthorizationStore : IOpenIddictAuthorizationStore<OpenIddictAuthorization>        
    {       
        Amazon.DynamoDBv2.IAmazonDynamoDB _dynamoDb;
        const string DBName = "OpenIdAuthorizations";

        public OpenIddictAuthorizationStore( Amazon.DynamoDBv2.IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        /// <summary>
        /// Creates a new authorization.
        /// </summary>
        /// <param name="authorization">The authorization to create.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation, whose result returns the authorization.
        /// </returns>
        public virtual async Task<OpenIddictAuthorization> CreateAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken)
        {
            if (authorization == null)
            {
                throw new ArgumentNullException(nameof(authorization));
            }

            if (String.IsNullOrWhiteSpace(authorization.Id)){
                authorization.Id = Guid.NewGuid().ToString("N");
            }

            await _dynamoDb.PutItemAsync(DBName,new Dictionary<string, AttributeValue> {
                    {"Id", new AttributeValue { S =  authorization.Id }},
                    {"Subject", new AttributeValue { S =  authorization.Subject }},
                    {"Scope", new AttributeValue { S =  authorization.Scope }},
                    {"ApplicationId", new AttributeValue { S =  authorization.ApplicationId }}});

            return authorization;
        }

        /// <summary>
        /// Retrieves an authorization using its associated subject/client.
        /// </summary>
        /// <param name="subject">The subject associated with the authorization.</param>
        /// <param name="client">The client associated with the authorization.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns the authorization corresponding to the subject/client.
        /// </returns>
        public virtual async Task<OpenIddictAuthorization> FindAsync(string subject, string client, CancellationToken cancellationToken)
        {            
            var key = ConvertIdentifierFromString(client);

            var result = await _dynamoDb.QueryAsync(new QueryRequest
            {
                TableName = DBName,
                KeyConditionExpression = "ApplicationId = :v_ApplicationId",
                FilterExpression = "Subject = :v_Subject",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":v_ApplicationId", new AttributeValue { S =  key }},
                    {":v_Subject", new AttributeValue { S =  subject }}}
            }, cancellationToken);

            var record = result.Items.FirstOrDefault();
            
            return OpenIdModelFactory.CreateAuthorizationFromAWS(record);
        }

        /// <summary>
        /// Retrieves an authorization using its unique identifier.
        /// </summary>
        /// <param name="identifier">The unique identifier associated with the authorization.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns the authorization corresponding to the identifier.
        /// </returns>
        public virtual async Task<OpenIddictAuthorization> FindByIdAsync(string identifier, CancellationToken cancellationToken)
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
            
            return OpenIdModelFactory.CreateAuthorizationFromAWS(record);
        }

        /// <summary>
        /// Retrieves the unique identifier associated with an authorization.
        /// </summary>
        /// <param name="authorization">The authorization.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns the unique identifier associated with the authorization.
        /// </returns>
        public virtual Task<string> GetIdAsync(OpenIddictAuthorization authorization, CancellationToken cancellationToken)
        {
            if (authorization == null)
            {
                throw new ArgumentNullException(nameof(authorization));
            }

            return Task.FromResult(ConvertIdentifierToString(authorization.Id));
        }

        /// <summary>
        /// Retrieves the subject associated with an authorization.
        /// </summary>
        /// <param name="authorization">The authorization.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns the subject associated with the specified authorization.
        /// </returns>
        public virtual Task<string> GetSubjectAsync( OpenIddictAuthorization authorization, CancellationToken cancellationToken)
        {
            if (authorization == null)
            {
                throw new ArgumentNullException(nameof(authorization));
            }

            return Task.FromResult(authorization.Subject);
        }

        /// <summary>
        /// Converts the provided identifier to a strongly typed key object.
        /// </summary>
        /// <param name="identifier">The identifier to convert.</param>
        /// <returns>An instance of <typeparamref name="string"/> representing the provided identifier.</returns>
        public virtual string ConvertIdentifierFromString( string identifier)
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
        public virtual string ConvertIdentifierToString( string identifier)
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