using System;

namespace Wealthperk.AWS.Models
{
    public class OpenIddictToken
    {
        /// <summary>
        /// Gets or sets the unique identifier
        /// associated with the current token.
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier
        /// associated with the current token.
        /// </summary>
        public virtual string ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the subject associated with the current token.
        /// </summary>
        public virtual string Subject { get; set; }

        /// <summary>
        /// Gets or sets the type of the current token.
        /// </summary>
        public virtual string Type { get; set; }
        
        /// <summary>
        /// Gets or sets the type of the current token.
        /// </summary>
        public string AuthorizationId { get; internal set; }
    }
}
