
using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using Wealthperk.Model;
using Wealthperk.Model.Profile;

namespace Wealthperk.AWS.Models
{
    public static class UserModelFactory
    {
        internal static UserInfo CreateUserInfoFromAWS(Dictionary<string, AttributeValue> record)
        {
            return new UserInfo {
                Id = record["Id"].N,
                UserName = record["UserName"].S,
                Email = record["Email"].S
            };
        }

        internal static UserInfo CreateUserInfoFromAWS(Document record)
        {
            return new UserInfo {
                Id = record["Id"].AsLong(),
                UserName = record["UserName"].AsString(),
                Email = record["Email"].AsString()
            };
        }

        internal static AccountInfo CreateAccountFromAWS(Document arg)
        {
            var rv = new AccountInfo {
                AccountId = arg["AccountId"].AsString()
            };

            if (arg.ContainsKey("DisplayName"))
                rv.DisplayName = arg["DisplayName"].AsString();

            if (arg.ContainsKey("SourceId"))
                rv.SourceId = arg["SourceId"].AsString();

            return rv;
        }

        internal static UserProfile CreateUserProfileFromAWS(Document item)
        {
            if (item == null)
                return null;
            return JsonConvert.DeserializeObject<UserProfile>(item.ToJson());
        }

        internal static PortfolioStrategy CreateUserSettingsFromAWS(Document item)
        {
            if (item == null)
                return null;
            return JsonConvert.DeserializeObject<PortfolioStrategy>(item.ToJson());
        }

        internal static Dictionary<string, AttributeValue> MapAccountInfoToAWS(AccountInfo x)
        {
            var res = new Dictionary<string, AttributeValue> {
                {"AccountId", new AttributeValue(x.AccountId)}
            };

            if (!string.IsNullOrWhiteSpace(x.DisplayName))
                res.Add("DisplayName", new AttributeValue(x.DisplayName));
            if (!string.IsNullOrWhiteSpace(x.DisplayName))
                res.Add("SourceId", new AttributeValue(x.SourceId));

            return res;
        }
    }

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
                ApplicationId = record["ApplicationId"]?.S,
                Scope = record["Scope"].S,
                Subject = record["Subject"].S,
                Id = record["Id"].S
            };
        }

        internal static OpenIddictToken CreateTokenFromAWS(Dictionary<string, AttributeValue> record)
        {
            return new OpenIddictToken() {
                ApplicationId = record["ApplicationId"]?.S,
                AuthorizationId = record["AuthorizationId"]?.S,
                Type = record["Type"].S,
                Subject = record["Subject"].S,
                Id = record["Id"].S
            };
        }
    }
}