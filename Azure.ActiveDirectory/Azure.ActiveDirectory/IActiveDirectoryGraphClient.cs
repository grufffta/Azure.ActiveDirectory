using Azure.ActiveDirectory.AzureGraphService.Microsoft.WindowsAzure.ActiveDirectory;
using System.Collections.Generic;
using System.Linq;

namespace Azure.ActiveDirectory
{
    /// <summary>
    /// Interface IActiveDirectoryGraphClient
    /// </summary>
    public interface IActiveDirectoryGraphClient
    {
        /// <summary>
        /// Gets the users in the active directory.
        /// </summary>
        /// <value>The users.</value>
        IQueryable<User> Users { get; }

        /// <summary>
        /// Gets the groups in the active directory.
        /// </summary>
        /// <value>The groups.</value>
        IQueryable<Group> Groups { get; }

        /// <summary>
        /// Gets the service principals.
        /// </summary>
        /// <value>The service principals.</value>
        IQueryable<ServicePrincipal> ServicePrincipals { get; }

        /// <summary>
        /// Gets the roles in the active directory.
        /// </summary>
        /// <value>The roles.</value>
        IQueryable<Role> Roles { get; }

        /// <summary>
        /// Gets the devices.
        /// </summary>
        /// <value>The devices.</value>
        IQueryable<Device> Devices { get; }

        /// <summary>
        /// Gets the applications.
        /// </summary>
        /// <value>The applications.</value>
        IQueryable<Application> Applications { get; }

        /// <summary>
        /// Gets the current user object in the active directory.
        /// </summary>
        /// <value>The current user.</value>
        User CurrentUser { get; }

        /// <summary>
        /// Gets the current groups assigned to the user.
        /// </summary>
        /// <value>The current user groups.</value>
        IEnumerable<Group> CurrentUserGroups { get; }

        /// <summary>
        /// Fetches the current user related directory objects.
        /// </summary>
        /// <typeparam name="TDirectoryObject">The type of the directory object to return.</typeparam>
        /// <returns>IEnumerable&lt;TDirectoryObject&gt;.</returns>
        IEnumerable<TDirectoryObject> FetchCurrentUser<TDirectoryObject>() where TDirectoryObject : DirectoryObject;

        /// <summary>
        /// Loads a related property on a directory object.
        /// </summary>
        /// <param name="directoryObject">The directory object.</param>
        /// <returns>DirectoryObject.</returns>
        DirectoryObject LoadPropertyMemberOf(DirectoryObject directoryObject);

        /// <summary>
        /// Adds the specified directory object to the active directory.
        /// </summary>
        /// <typeparam name="TDirectoryObject">The type of the t directory object.</typeparam>
        /// <param name="directoryObjectToAdd">The directory object to add.</param>
        void Add<TDirectoryObject>(TDirectoryObject directoryObjectToAdd) where TDirectoryObject : DirectoryObject;

        /// <summary>
        /// Updates the specified directory object in the active directory.
        /// </summary>
        /// <typeparam name="TDirectoryObject">The type of the t directory object.</typeparam>
        /// <param name="directoryObjectToUpdate">The directory object to update.</param>

        void Update<TDirectoryObject>(TDirectoryObject directoryObjectToUpdate) where TDirectoryObject : DirectoryObject;

        /// <summary>
        /// Deletes the specified directory object from the active directory.
        /// </summary>
        /// <typeparam name="TDirectoryObject">The type of the t directory object.</typeparam>
        /// <param name="directoryObjectToDelete">The directory object to delete.</param>
        void Delete<TDirectoryObject>(TDirectoryObject directoryObjectToDelete) where TDirectoryObject : DirectoryObject;
    }
}