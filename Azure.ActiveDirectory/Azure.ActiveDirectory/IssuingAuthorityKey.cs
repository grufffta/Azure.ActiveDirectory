using Microsoft.WindowsAzure.Storage.Table;

namespace Azure.ActiveDirectory
{
    /// <summary>
    /// Class IssuingAuthorityKey. This class cannot be inherited.
    /// </summary>
    public sealed class IssuingAuthorityKey : TableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IssuingAuthorityKey"/> class.
        /// </summary>
        /// <param name="issuer">The issuer.</param>
        /// <param name="thumbprint">The thumbprint.</param>
        public IssuingAuthorityKey(string issuer, string thumbprint)
        {
            PartitionKey = issuer;
            RowKey = thumbprint;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IssuingAuthorityKey"/> class.
        /// </summary>
        public IssuingAuthorityKey()
        {
        }

        /// <summary>
        /// Gets or sets the issuer.
        /// </summary>
        /// <value>The issuer.</value>
        public string Issuer { get { return PartitionKey; } set { PartitionKey = value; } }

        /// <summary>
        /// Gets or sets the thumbprint.
        /// </summary>
        /// <value>The thumbprint.</value>
        public string Thumbprint { get { return RowKey; } set { RowKey = value; } }
    }
}