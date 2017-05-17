
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
    public class OpenIddictApplicationStore: IOpenIddictApplicationStore<OpenIddictApplication>
    {             

        Amazon.DynamoDBv2.IAmazonDynamoDB _dynamoDb;

        public OpenIddictApplicationStore(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        /// <summary>
        /// Creates a new application.
        /// </summary>
        /// <param name="application">The application to create.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation, whose result returns the application.
        /// </returns>
        public virtual async Task<OpenIddictApplication> CreateAsync(OpenIddictApplication application, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes an existing application.
        /// </summary>
        /// <param name="application">The application to delete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation.
        /// </returns>
        public virtual async Task DeleteAsync(OpenIddictApplication application, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new NotSupportedException();
        }

        /// <summary>
        /// Retrieves an application using its unique identifier.
        /// </summary>
        /// <param name="identifier">The unique identifier associated with the application.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns the client application corresponding to the identifier.
        /// </returns>
        public virtual async Task<OpenIddictApplication> FindByIdAsync(string identifier, CancellationToken cancellationToken)
        {
            var result = await _dynamoDb.QueryAsync(new QueryRequest
            {
                TableName = "OpenIdApplication",
                KeyConditionExpression = "Id = :v_Id",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":v_Id", new AttributeValue { S =  identifier }}}
            }, cancellationToken);

            var record = result.Items.FirstOrDefault();
            
            return OpenIdModelFactory.CreateApplicationFromAWS(record);

        }

        /// <summary>
        /// Retrieves an application using its client identifier.
        /// </summary>
        /// <param name="identifier">The client identifier associated with the application.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns the client application corresponding to the identifier.
        /// </returns>
        public virtual async Task<OpenIddictApplication> FindByClientIdAsync(string identifier, CancellationToken cancellationToken)
        {
            var result = await _dynamoDb.QueryAsync(new QueryRequest
            {
                TableName = "OpenIdApplication",
                KeyConditionExpression = "ClientId = :v_ClientId",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":v_ClientId", new AttributeValue { S =  identifier }}}
            }, cancellationToken);

            var record = result.Items.FirstOrDefault();
            
            return OpenIdModelFactory.CreateApplicationFromAWS(record);
        }

        /// <summary>
        /// Retrieves an application using its post_logout_redirect_uri.
        /// </summary>
        /// <param name="url">The post_logout_redirect_uri associated with the application.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation, whose result
        /// returns the client application corresponding to the post_logout_redirect_uri.
        /// </returns>
        public virtual Task<OpenIddictApplication> FindByLogoutRedirectUri(string url, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Retrieves the client identifier associated with an application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns the client identifier associated with the application.
        /// </returns>
        public virtual Task<string> GetClientIdAsync( OpenIddictApplication application, CancellationToken cancellationToken)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return Task.FromResult(application.ClientId);
        }

        /// <summary>
        /// Retrieves the client type associated with an application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns the client type of the application (by default, "public").
        /// </returns>
        public virtual Task<string> GetClientTypeAsync( OpenIddictApplication application, CancellationToken cancellationToken)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return Task.FromResult(application.Type);
        }

        /// <summary>
        /// Retrieves the display name associated with an application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns the display name associated with the application.
        /// </returns>
        public virtual Task<string> GetDisplayNameAsync( OpenIddictApplication application, CancellationToken cancellationToken)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return Task.FromResult(application.DisplayName);
        }

        /// <summary>
        /// Retrieves the hashed secret associated with an application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns the hashed secret associated with the application.
        /// </returns>
        public virtual Task<string> GetHashedSecretAsync( OpenIddictApplication application, CancellationToken cancellationToken)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return Task.FromResult(application.ClientSecret);
        }

        /// <summary>
        /// Retrieves the unique identifier associated with an application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns the unique identifier associated with the application.
        /// </returns>
        public virtual Task<string> GetIdAsync( OpenIddictApplication application, CancellationToken cancellationToken)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return Task.FromResult(ConvertIdentifierToString(application.Id));
        }

        /// <summary>
        /// Retrieves the logout callback address associated with an application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns the post_logout_redirect_uri associated with the application.
        /// </returns>
        public virtual Task<string> GetLogoutRedirectUriAsync( OpenIddictApplication application, CancellationToken cancellationToken)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return Task.FromResult(application.LogoutRedirectUri);
        }

        /// <summary>
        /// Retrieves the callback address associated with an application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns the redirect_uri associated with the application.
        /// </returns>
        public virtual Task<string> GetRedirectUriAsync( OpenIddictApplication application, CancellationToken cancellationToken)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return Task.FromResult(application.RedirectUri);
        }

        /// <summary>
        /// Retrieves the token identifiers associated with an application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns the tokens associated with the application.
        /// </returns>
        public virtual async Task<IEnumerable<string>> GetTokensAsync( OpenIddictApplication application, CancellationToken cancellationToken)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            var result = await _dynamoDb.QueryAsync(new QueryRequest
            {
                TableName = "OpenIdTokens",
                KeyConditionExpression = "ApplicationId = :v_ApplicationId",
                ProjectionExpression = "Id",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":v_ApplicationId", new AttributeValue { S =  application.Id }}}
                
            }, cancellationToken);
           
            var tokens = new List<string>();

            foreach (var identifier in result.Items.ToArray())
            {
                tokens.Add(ConvertIdentifierToString(identifier["Id"].S));
            }

            return tokens;
        }

        /// <summary>
        /// Sets the client type associated with an application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="type">The client type associated with the application.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation.
        /// </returns>
        public virtual Task SetClientTypeAsync( OpenIddictApplication application,  string type, CancellationToken cancellationToken)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentException("The client type cannot be null or empty.", nameof(type));
            }

            application.Type = type;

            return Task.FromResult(0);
        }

        /// <summary>
        /// Sets the hashed secret associated with an application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="hash">The hashed client secret associated with the application.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation.
        /// </returns>
        public virtual Task SetHashedSecretAsync( OpenIddictApplication application,  string hash, CancellationToken cancellationToken)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            if (string.IsNullOrEmpty(hash))
            {
                throw new ArgumentException("The client secret hash cannot be null or empty.", nameof(hash));
            }

            application.ClientSecret = hash;

            return Task.FromResult(0);
        }

        /// <summary>
        /// Updates an existing application.
        /// </summary>
        /// <param name="application">The application to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation.
        /// </returns>
        public virtual async Task UpdateAsync(OpenIddictApplication application, CancellationToken cancellationToken)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            Context.Attach(application);
            Context.Update(application);

            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }

            catch (Exception) { }
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