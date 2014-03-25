using System.Security.Claims;

namespace Azure.ActiveDirectory
{
    /// <summary>
    /// Class GraphClaimsAuthenticationManager.
    /// Provides roles based authentication (IsInRole(...)) using the Azure active directory group membership
    /// </summary>
    public class GraphClaimsAuthenticationManager : ClaimsAuthenticationManager
    {
        /// <summary>
        /// When overridden in a derived class, returns a <see cref="T:System.Security.Claims.ClaimsPrincipal" /> object consistent with the requirements of the RP application. The default implementation does not modify the incoming <see cref="T:System.Security.Claims.ClaimsPrincipal" />.
        /// </summary>
        /// <param name="resourceName">The address of the resource that is being requested.</param>
        /// <param name="incomingPrincipal">The claims principal that represents the authenticated user that is attempting to access the resource.</param>
        /// <returns>A claims principal that contains any modifications necessary for the RP application. The default implementation returns the incoming claims principal unmodified.</returns>
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            var client = new GraphClient(incomingPrincipal);
            foreach (var group in client.CurrentUserGroups)
            {
                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(new Claim(ClaimTypes.Role, group.displayName, ClaimValueTypes.String, client.TenantId));
            }
            return base.Authenticate(resourceName, incomingPrincipal);
        }
    }
}