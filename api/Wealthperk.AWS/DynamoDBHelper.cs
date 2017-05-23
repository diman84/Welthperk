using Amazon.DynamoDBv2.Model;

namespace Wealthperk.AWS
{
    public class AWSHelper {
        public static AttributeValue CreateAttributeVale(string value){
          return string.IsNullOrWhiteSpace(value)
            ? new AttributeValue{NULL = true}
            : new AttributeValue(value);
        }
    }
}