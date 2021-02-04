using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Server.Interfaces
{
    public interface ILibraryRepository : IRepositoryBase<Shared.Models.Library>
    {
        /// <summary>
        /// Get all libraries in the database.
        /// </summary>
        /// <returns>IEnumerable containing all libraries.</returns>
        Task<IEnumerable<Shared.Models.Library>> GetAllLibraries();
        /// <summary>
        /// Get library by its id.
        /// </summary>
        /// <param name="id">Id of the library.</param>
        /// <returns>Library found.</returns>
        Task<Shared.Models.Library> GetLibraryById(int id);
        /// <summary>
        /// Get libraries by name. It uses SQL LIKE statement.
        /// </summary>
        /// <param name="name">Name to look for.</param>
        /// <returns>IEnumerable containing all libraries found or null</returns>
        Task<IEnumerable<Shared.Models.Library>> GetLibrariesByName(string name);
        /// <summary>
        /// Get a library by name. Must match exactly.
        /// NOT casesensitive.
        /// </summary>
        /// <param name="name">Name of the library.</param>
        /// <returns>Library found or null.</returns>
        Task<Shared.Models.Library> GetLibraryByName(string name);
        /// <summary>
        /// Add instance to the context.
        /// </summary>
        /// <param name="library">Instance to add.</param>
        void CreateLibrary(Shared.Models.Library library);
        /// <summary>
        /// Update instance in the context.
        /// </summary>
        /// <param name="library">Instance to update.</param>
        void UpdateLibrary(Shared.Models.Library library);
        /// <summary>
        /// Removes an instance from the context.
        /// </summary>
        /// <param name="library">Instance to remove.</param>
        void DeleteLibrary(Shared.Models.Library library);
    }
}
