using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;

namespace Azure.ActiveDirectory
{
    /// <summary>
    /// Class AzureTableIssuerNameRegistry.
    /// Stores issuing authority thumbrints
    /// </summary>
    public class AzureTableIssuerNameRegistry : ValidatingIssuerNameRegistry
    {
        /// <summary>
        /// The table
        /// </summary>
        private static readonly CloudTable Table;

        /// <summary>
        /// Initializes static members of the <see cref="AzureTableIssuerNameRegistry"/> class.
        /// </summary>
        static AzureTableIssuerNameRegistry()
        {
            var client = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("Identity.StorageConnectionString")).CreateCloudTableClient();
            Table = client.GetTableReference("IssuingAuthorityThumbprint");
            Table.CreateIfNotExists();
        }

        /// <summary>
        /// Refreshes the keys.
        /// </summary>
        /// <param name="metadataLocation">The metadata location.</param>
        public async static void RefreshKeys(string metadataLocation)
        {
            var issuingAuthority = GetIssuingAuthority(metadataLocation);
            var newKeys = issuingAuthority.Thumbprints.Any(thumbprint => !ContainsKey(issuingAuthority.Name.TrimEnd('/').Split('/').Last(), thumbprint));
            if (!newKeys) return;
            foreach (var upsert in issuingAuthority.Thumbprints.Select(thumbprint => new IssuingAuthorityKey(issuingAuthority.Name.TrimEnd('/').Split('/').Last(), thumbprint)).Select(TableOperation.InsertOrReplace))
            {
                await Table.ExecuteAsync(upsert);
            }
        }

        /// <summary>
        /// Determines whether the specified issuer contains key.
        /// </summary>
        /// <param name="issuer">The issuer.</param>
        /// <param name="thumbPrint">The thumb print.</param>
        /// <returns><c>true</c> if the specified issuer contains key; otherwise, <c>false</c>.</returns>
        public static bool ContainsKey(string issuer, string thumbPrint)
        {
            var result = Table.Execute(TableOperation.Retrieve<IssuingAuthorityKey>(issuer, thumbPrint));
            return result.HttpStatusCode == (int)HttpStatusCode.OK;
        }

        /// <summary>
        /// Determines whether the specified thumbprint is valid for the issuer.
        /// </summary>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <param name="issuer">The issuer.</param>
        /// <returns><c>true</c> if the thumbprint is valid; otherwise, <c>false</c>.</returns>
        protected override bool IsThumbprintValid(string thumbprint, string issuer)
        {
            return ContainsKey(issuer.TrimEnd('/').Split('/').Last(), thumbprint);
        }
    }
}