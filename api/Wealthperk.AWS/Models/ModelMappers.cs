
using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;

namespace Wealthperk.AWS.Models
{
    public static class OpenIdModelFactory
    {
        internal static OpenIddictApplication CreateApplicationFromAWS(Dictionary<string, AttributeValue> record)
        {
            return new OpenIddictApplication() {
                ClientId = record["ClientId"].S,
                ClientSecret = record["ClientSecret"].S,
                DisplayName = record["DisplayName"].S,
                RedirectUri = record["RedirectUri"].S,
                Type = record["Type"].S,
                Id = record["Id"].S                 
            };
        }

        internal static OpenIddictAuthorization CreateAuthorizationFromAWS(Dictionary<string, AttributeValue> record)
        {
            return new OpenIddictAuthorization() {
                ApplicationId = record["ApplicationId"].S,
                Scope = record["Scope"].S,
                Subject = record["Subject"].S,
                Id = record["Id"].S                 
            };
        }

        internal static OpenIddictToken CreateTokenFromAWS(Dictionary<string, AttributeValue> record)
        {
            return new OpenIddictToken() {
                ApplicationId = record["ApplicationId"].S,
                AuthorizationId = record["AuthorizationId"].S,
                Type = record["Type"].S,
                Subject = record["Subject"].S,
                Id = record["Id"].S                 
            };
        }
    }
}