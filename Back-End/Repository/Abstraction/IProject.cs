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

        public Task<object> AddProject(List<ProjectViewModel> viewModel, string Userid);
        public Task<object> AllocateResources(MapProjectResources viewModel);

        public bool DeleteActivity(int activityID);
        public Task<object> AddActivity(List<ActivityMaster> viewModel);
        public bool DeleteProject(int ProjectID);

        public Task<object> EditProject(EditProjectView project);
        public Task<object> EditProjectDetails(EditProjectView project);
        public Task<object> EditProjectActive(EditProjectView project);

        public Task<object> EditActivity(List<ActivityMaster> viewModel);
        public Task<List<ClientViewModel>> GetClientList();
        public Task<List<ProjectListViewModel>> GetAllProject();
        public Task<List<ProjectListViewModel>> GetProjectList(string EmployeeID, string ProjectName);
        public Task<ProjectViewModel> GetprojectDetailsByID(int ProjectID);
    }
}
