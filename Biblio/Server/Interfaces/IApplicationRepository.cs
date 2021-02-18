using Biblio.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Server.Interfaces
{
    public interface IApplicationRepository : IRepositoryBase<Application>
    {
        Task<List<Application>> GetAllOpenApplications();
        Task<Application> GetApplicationById(int id);
        void CreateApplication(Application application);
        void UpdateApplication(Application application);
        void DeleteApplication(Application application);
    }
}
