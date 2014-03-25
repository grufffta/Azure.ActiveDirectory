using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// ReSharper disable CheckNamespace
namespace Azure.ActiveDirectory.AzureGraphService.Microsoft.WindowsAzure.ActiveDirectory
// ReSharper restore CheckNamespace
{

    /// <summary>
    /// Class User. Extends the data service user to include the 'userType' and 'immutableId' properties.
    /// </summary>
    [MetadataType(typeof(UserMetadata))]
    partial class User
    {
        /// <summary>
        /// Gets or sets the immutable identifier.
        /// </summary>
        /// <value>The immutable identifier.</value>
        public string immutableId { get; set; }
        /// <summary>
        /// Gets or sets the type of the user.
        /// </summary>
        /// <value>The type of the user.</value>
        public string userType { get; set; }

        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Class UserMetadata.
        /// Data annotations for naming/data types.
        /// </summary>
        internal class UserMetadata
        {
            /// <summary>
            /// Gets or sets the name of the given.
            /// </summary>
            /// <value>The name of the given.</value>
            [DisplayName("name")]
            public string givenName { get; set; }

            /// <summary>
            /// Gets or sets the mail.
            /// </summary>
            /// <value>The mail.</value>
            [DisplayName("e-mail")]
            [DataType(DataType.EmailAddress)]
            public string mail { get; set; }

            /// <summary>
            /// Gets or sets the job title.
            /// </summary>
            /// <value>The job title.</value>
            [DisplayName("job title")]
            public string jobTitle { get; set; }

            /// <summary>
            /// Gets or sets the telephone number.
            /// </summary>
            /// <value>The telephone number.</value>
            [DisplayName("telephone")]
            [DataType(DataType.PhoneNumber)]
            public string telephoneNumber { get; set; }

            /// <summary>
            /// Gets or sets the mobile.
            /// </summary>
            /// <value>The mobile.</value>
            [DisplayName("mobile")]
            [DataType(DataType.PhoneNumber)]
            public string mobile { get; set; }

            /// <summary>
            /// Gets or sets the facsimile telephone number.
            /// </summary>
            /// <value>The facsimile telephone number.</value>
            [DisplayName("fax")]
            [DataType(DataType.PhoneNumber)]
            public string facsimileTelephoneNumber { get; set; }

            /// <summary>
            /// Gets or sets the street address.
            /// </summary>
            /// <value>The street address.</value>
            [DisplayName("address")]
            [DataType(DataType.MultilineText)]
            public string streetAddress { get; set; }

            /// <summary>
            /// Gets or sets the state.
            /// </summary>
            /// <value>The state.</value>
            [DisplayName("county")]
            public string state { get; set; }

            /// <summary>
            /// Gets or sets the postal code.
            /// </summary>
            /// <value>The postal code.</value>
            [DisplayName("postCode")]
            [DataType(DataType.PostalCode)]
            public string postalCode { get; set; }
            // ReSharper restore InconsistentNaming
        }
    }    
}
// ReSharper restore InconsistentNaming
