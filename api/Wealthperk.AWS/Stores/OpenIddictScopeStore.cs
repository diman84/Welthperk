using System;
using System.ComponentModel;
using OpenIddict.Core;
using OpenIddict.Models;

namespace Wealthperk.AWS
{
    public class OpenIddictScopeStore : IOpenIddictScopeStore<OpenIddictScope>
    {        

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