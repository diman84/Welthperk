using System;
using System.Collections.Generic;

namespace Wealthperk.AWS.Models
{  
    public class OpenIddictAuthorization
    {        

        /// <summary>
        /// Gets or sets the unique identifier
        /// associated with the current authorization.
        /// </summary>
        public virtual string Id { get; set; }


        /// <summary>
        /// Gets or sets the unique identifier
        /// associated with the current authorization.
        /// </summary>
        public virtual string ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the space-delimited scopes
        /// associated with the current authorization.
        /// </summary>
        public virtual string Scope { get; set; }

        /// <summary>
        /// Gets or sets the subject associated with the current authorization.
        /// </summary>
        public virtual string Subject { get; set; }    
    }
}
