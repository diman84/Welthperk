using System;
using Amazon.DynamoDBv2.Model;

namespace Wealthperk.AWS
{
    public static class DynamoDBHelper {
        public static AttributeValue CreateAttributeVale(string value){
          return string.IsNullOrWhiteSpace(value)
            ? new AttributeValue{NULL = true}
            : new AttributeValue(value);
        }

        public static string ToAWSDate(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
    }
}