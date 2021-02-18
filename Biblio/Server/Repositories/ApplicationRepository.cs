using Biblio.Server.Data;
using Biblio.Server.Interfaces;
using Biblio.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Server.Repositories
{
    public class ApplicationRepository : RepositoryBase<Application>, IApplicationRepository
    {
        public ApplicationRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext) { }


        public void CreateApplication(Application application)
        {
            Create(application);
        }

        public void DeleteApplication(Application application)
        {
            Delete(application);
        }

        public async Task<List<Application>> GetAllOpenApplications()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<Application> GetApplicationById(int id)
        {
            return await FindByPrimaryKey(id);
        }

        public void UpdateApplication(Application application)
        {
            Update(application);
        }
    }
}
