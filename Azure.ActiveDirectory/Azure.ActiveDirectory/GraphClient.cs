using Azure.ActiveDirectory.AzureGraphService.Microsoft.WindowsAzure.ActiveDirectory;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Security.Claims;

namespace Azure.ActiveDirectory
{
    /// <summary>
    /// Class GraphClient for interacting with the Azure active directory.
    /// </summary>
    public class GraphClient : IActiveDirectoryGraphClient
    {
        /// <summary>
        /// The tenant identifier claim type
        /// </summary>
        private const string TenantIdClaimType = "http://schemas.microsoft.com/identity/claims/tenantid";

        /// <summary>
        /// The user object identifier claim type
        /// </summary>
        private const string UserObjectIdClaimType = "http://schemas.microsoft.com/identity/claims/objectidentifier";

        /// <summary>
        /// The login URL
        /// </summary>
        private const string LoginUrl = "https://login.windows.net/{0}";

        /// <summary>
        /// The graph authentication resource
        /// </summary>
        private const string GraphAuthResource = "https://graph.windows.net";

        /// <summary>
        /// The graph URL
        /// </summary>
        private const string GraphUrl = "https://graph.windows.net/{0}";

        /// <summary>
        /// The authorisation header
        /// </summary>
        private const string AuthorisationHeader = "Authorization";

        /// <summary>
        /// The tenant identifier
        /// </summary>
        public readonly string TenantId;

        /// <summary>
        /// The _user object identifier
        /// </summary>
        private readonly string _userObjectId;

        /// <summary>
        /// The application principal identifier
        /// </summary>
        private static readonly string AppPrincipalId = CloudConfigurationManager.GetSetting("ida:ClientID");

        /// <summary>
        /// The application key
        /// </summary>
        private static readonly string AppKey = CloudConfigurationManager.GetSetting("ida:Password");

        /// <summary>
        /// The _service
        /// </summary>
        private readonly DirectoryDataService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphClient"/> class.
        /// </summary>
        public GraphClient()
            : this(ClaimsPrincipal.Current)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphClient"/> class.
        /// </summary>
        /// <param name="incomingPrincipal">The incoming principal.</param>
        public GraphClient(ClaimsPrincipal incomingPrincipal)
        {
            if (!incomingPrincipal.Identity.IsAuthenticated) return;

            TenantId = incomingPrincipal.FindFirst(TenantIdClaimType).Value;
            _userObjectId = incomingPrincipal.FindFirst(UserObjectIdClaimType).Value;
            _service = new DirectoryDataService(new Uri(GetGraphUrl()));
            _service.BuildingRequest += (s, e) =>
            {
                e.RequestUri = !string.IsNullOrEmpty(e.RequestUri.Query)
                    ? new Uri(string.Concat(e.RequestUri, "&api-version=2013-11-08"))
                    : new Uri(string.Concat(e.RequestUri, "?api-version=2013-11-08"));
                e.Headers.Add(AuthorisationHeader, GetAuthorizationHeader());
            };
        }

        /// <summary>
        /// Gets the graph URL.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetGraphUrl()
        {
            return string.Format(GraphUrl, TenantId);
        }

        /// <summary>
        /// Gets the users in the active directory.
        /// </summary>
        /// <value>The users.</value>
        public IQueryable<User> Users { get { return _service.directoryObjects.OfType<User>(); } }

        /// <summary>
        /// Gets the groups in the active directory.
        /// </summary>
        /// <value>The groups.</value>
        public IQueryable<Group> Groups { get { return _service.directoryObjects.OfType<Group>(); } }

        /// <summary>
        /// Gets the contacts.
        /// </summary>
        /// <value>The contacts.</value>
        public IQueryable<Contact> Contacts { get { return _service.directoryObjects.OfType<Contact>(); } }

        /// <summary>
        /// Gets the service principals.
        /// </summary>
        /// <value>The service principals.</value>
        public IQueryable<ServicePrincipal> ServicePrincipals { get { return _service.directoryObjects.OfType<ServicePrincipal>(); } }

        /// <summary>
        /// Gets the roles in the active directory.
        /// </summary>
        /// <value>The roles.</value>
        public IQueryable<Role> Roles { get { return _service.directoryObjects.OfType<Role>(); } }

        /// <summary>
        /// Gets the devices.
        /// </summary>
        /// <value>The devices.</value>
        public IQueryable<Device> Devices { get { return _service.directoryObjects.OfType<Device>(); } }

        /// <summary>
        /// Gets the applications.
        /// </summary>
        /// <value>The applications.</value>
        public IQueryable<Application> Applications { get { return _service.directoryObjects.OfType<Application>(); } }

        // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault // First/Single not compaitble
        /// <summary>
        /// Gets the current user object in the active directory.
        /// </summary>
        /// <value>The current user.</value>
        public User CurrentUser { get { return _service.directoryObjects.OfType<User>().Where(q => q.objectId == _userObjectId).FirstOrDefault(); } }

        /// <summary>
        /// Gets the current groups assigned to the user.
        /// </summary>
        /// <value>The current user groups.</value>
        public IEnumerable<Group> CurrentUserGroups { get { return FetchCurrentUser<Group>(); } }

        /// <summary>
        /// Fetches the current user related directory objects.
        /// </summary>
        /// <typeparam name="TDirectoryObject">The type of the directory object to return.</typeparam>
        /// <returns>IEnumerable&lt;TDirectoryObject&gt;.</returns>
        public IEnumerable<TDirectoryObject> FetchCurrentUser<TDirectoryObject>() where TDirectoryObject : DirectoryObject
        {
            return LoadPropertyMemberOf(CurrentUser).memberOf.OfType<TDirectoryObject>();
        }

        /// <summary>
        /// Loads a related property on a directory object.
        /// </summary>
        /// <param name="directoryObject">The directory object.</param>
        /// <returns>DirectoryObject.</returns>
        public DirectoryObject LoadPropertyMemberOf(DirectoryObject directoryObject)
        {
            _service.LoadProperty(directoryObject, "memberOf");
            return directoryObject;
        }

        /// <summary>
        /// Gets the authorization header.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetAuthorizationHeader()
        {
            var authContext = new AuthenticationContext(String.Format(LoginUrl, TenantId));
            var credential = new ClientCredential(AppPrincipalId, AppKey);
            var assertionCredential = authContext.AcquireToken(GraphAuthResource, credential);
            return assertionCredential.CreateAuthorizationHeader();
        }

        /// <summary>
        /// Adds the specified directory object to the active directory.
        /// </summary>
        /// <typeparam name="TDirectoryObject">The type of the t directory object.</typeparam>
        /// <param name="directoryObjectToAdd">The directory object to add.</param>
        public void Add<TDirectoryObject>(TDirectoryObject directoryObjectToAdd) where TDirectoryObject : DirectoryObject
        {
            _service.AddTodirectoryObjects(directoryObjectToAdd);
            _service.SaveChanges();
        }

        /// <summary>
        /// Updates the specified directory object in the active directory.
        /// </summary>
        /// <typeparam name="TDirectoryObject">The type of the t directory object.</typeparam>
        /// <param name="directoryObjectToUpdate">The directory object to update.</param>
        public void Update<TDirectoryObject>(TDirectoryObject directoryObjectToUpdate) where TDirectoryObject : DirectoryObject
        {
            _service.UpdateObject(directoryObjectToUpdate);
            _service.SaveChanges(SaveChangesOptions.PatchOnUpdate);
        }

        /// <summary>
        /// Deletes the specified directory object from the active directory.
        /// </summary>
        /// <typeparam name="TDirectoryObject">The type of the t directory object.</typeparam>
        /// <param name="directoryObjectToDelete">The directory object to delete.</param>
        public void Delete<TDirectoryObject>(TDirectoryObject directoryObjectToDelete) where TDirectoryObject : DirectoryObject
        {
            _service.DeleteObject(directoryObjectToDelete);
            _service.SaveChanges();
        }
    }
}