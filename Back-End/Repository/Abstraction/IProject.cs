using AnL.Models;
using AnL.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnL.Repository.Abstraction
{
    public interface IProject : IRepository<ProjectDetails>
    {

        public Task<List<ProjectViewModel>> GetprojectListbyEmployeeID(string EmpID);

        public Task<List<ActivityMaster>> GetActivityList();

        public Task<object> AddProject(List<ProjectViewModel> viewModel);
        public Task<object> AllocateResources(MapProjectResources viewModel);

        public bool DeleteProject(List<ProjectViewModel> viewModel);
        public Task<object> AddActivity(List<ActivityMaster> viewModel);

    }
}
