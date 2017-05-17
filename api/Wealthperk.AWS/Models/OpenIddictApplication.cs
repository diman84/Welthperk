using System;
using System.Collections.Generic;
using System.Linq;

namespace Wealthperk.AWS.Models
{
        /// <summary>
    /// Represents an OpenIddict application.
    /// </summary>
    public class OpenIddictApplication
    {
        /// <summary>
        /// Gets or sets the client identifier
        /// associated with the current application.
        /// </summary>
        public virtual string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the hashed client secret
        /// associated with the current application.
        /// </summary>
        public virtual string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the display name
        /// associated with the current application.
        /// </summary>
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier
        /// associated with the current application.
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// Gets or sets the logout callback URL
        /// associated with the current application.
        /// </summary>
        public virtual string LogoutRedirectUri { get; set; }

        /// <summary>
        /// Gets or sets the callback URL
        /// associated with the current application.
        /// </summary>
        public virtual string RedirectUri { get; set; }       

        /// <summary>
        /// Gets or sets the application type
        /// associated with the current application.
        /// </summary>
        public virtual string Type { get; set; }
    }
}