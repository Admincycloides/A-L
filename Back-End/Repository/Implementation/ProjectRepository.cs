using AnL.Models;
using AnL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AnL.ViewModel;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace AnL.Repository.Implementation
{
    public class ProjectRepository : Repository<ProjectDetails>, IProject
    {
        private DbContext _context;
        private readonly IUnitOfWork _UOW;
        private TimesheetDetailRepository timesheetDetail;

        DbSet<ProjectDetails> dbSet;
        DbSet<ProjectMapping> dbSetProjectMapp;
        DbSet<ActivityMapping> dbActivityMapp;
        DbSet<ActivityDetails> dbActivity;
        DbSet<ClientDetails> dbClient;
        public ProjectRepository(DbContext context, IUnitOfWork UOW) : base(context)
        {
            this._context = context;
            this._UOW = UOW;
            dbSet = context.Set<ProjectDetails>();
            dbSetProjectMapp= context.Set<ProjectMapping>();
            dbActivityMapp = context.Set<ActivityMapping>();
            dbActivity = context.Set<ActivityDetails>();
            timesheetDetail = new TimesheetDetailRepository(context,UOW);
        }

        public async Task<object> AllocateResources(MapProjectResources Data)
        {
            List<string> Ids = new List<string>();
            Ids.AddRange(Data.SupervisorIds); Ids.AddRange(Data.EmployeeIds);
            var mapper = new List<ProjectMapping>();
            var ad = _context.Set<EmployeeDetails>().Where(X => Ids.Contains(X.EmployeeId)).ToList();
            try
            {
                if (_context.Set<EmployeeDetails>().Where(X => Ids.Contains(X.EmployeeId)).Count()!=Ids.Count())
                {
                    return null;
                }
                await Task.Run(() =>
                {
                    foreach (var s in Ids)
                    {
                        mapper.Add(new ProjectMapping
                        {
                            Active = true,
                            EmployeeId = s,
                            LastUpdate = DateTime.UtcNow,
                            LastUpdatedBy = Data.LoggedUserID,
                            ProjectId = Data.ProjectID


                        });
                    }

                    this.dbSetProjectMapp.AddRange(mapper);
                    this.SaveChanges();
                });
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        public async Task<object> AddProject(List<ProjectViewModel> viewModel)
        {
            try
            {
                foreach(var proj in viewModel)
                {
                    if (!await _context.Set<ProjectDetails>().Where(X => X.ProjectName.Trim() == proj.ProjectName.Trim().ToLower()).AnyAsync())
                    {

                        var activty = new List<ActivityMapping>();


                        foreach (var a in proj.Activities)
                        {
                            activty.Add(new ActivityMapping { ActivityId = a.ActivityId, IsActive=true});
                        }

                        ProjectDetails Project = new ProjectDetails
                        {
                            ClientId = proj.ClientId,
                            CurrentStatus = proj.CurrentStatus,
                            EnabledFlag = proj.EnabledFlag,
                            EndDate = proj.EndDate,
                            ProjectDescription = proj.ProjectDescription,
                            ProjectName = proj.ProjectName,
                            SredProject = proj.SredProject,
                            StartDate = proj.StartDate,
                            ActivityMapping = activty

                        };

                        this.Add(Project);
                        this.SaveChanges();
                        
                        //foreach (var a in proj.Activities)
                        //{
                        //    activty.Add(new ActivityMapping { ActivityId = a.ActivityId, ProjectId= Project.ProjectId });
                        //}
                        //Project.ActivityMapping = activty;
                        //this.SaveChanges();
                        return (await _context.Set<ProjectDetails>().Where(X => X.ProjectName.Trim() == proj.ProjectName.Trim().ToLower()).Select(X => X.ProjectId).FirstOrDefaultAsync());
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ActivityMaster>> GetActivityList()
        {
            List<ActivityMaster> rsp = new List<ActivityMaster>();
            try
            {
                await Task.Run(() =>
               {
                   rsp = (_context.Set<ActivityDetails>().Select(

                       X => new ActivityMaster
                       {
                           ActivityDescription = X.ActivityDescription,
                           ActivityId = X.ActivityId,
                           ActivityName = X.ActivityName,
                           EnabledFlag = X.EnabledFlag


                       }
                       )).ToList();

               });
                return rsp;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            }
        public async Task<List<ClientViewModel>> GetClientList()
        {
            List<ClientViewModel> rsp = new List<ClientViewModel>();
            try
            {
                await Task.Run(() =>
                {
                    rsp = (_context.Set<ClientDetails>().Select(

                        X => new ClientViewModel
                        {
                            ClientId=X.ClientId,
                            ClientName=X.ClientName,
                            ContactEmailAddress=X.ContactEmailAddress,
                            PointOfContactName=X.PointOfContactName,
                            Address=X.Address,
                            ContactNumber=X.ContactNumber

                        }
                        )).ToList();

                });
                return rsp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<object> EditProject(EditProjectView project)
        {
            try
            {
                List<int> Ids = new List<int>();
                //validation
                //var projectDetails = !await _context.Set<ProjectDetails>().Where(X => X.ProjectId == project.ProjectId).AnyAsync();
                //if (projectDetails)
                //    return "Project dose not exist";


                if (project.NewActivity != null)
                    if (project.NewActivity.Count > 0)
                    {
                        var activty = new List<ActivityMapping>();
                        foreach (var a in project.NewActivity)
                        {
                            activty.Add(new ActivityMapping { ActivityId = a.ActivityId, ProjectId=project.ProjectId });
                        }
                        this.dbActivityMapp.AddRange(activty);
                        this.SaveChanges();
                    }


                var NewProject = this.dbSet.Where(X=>X.ProjectId== project.ProjectId).FirstOrDefault();
                 NewProject.ProjectDescription = project.ProjectDescription;
                NewProject.ProjectName = project.ProjectName;
                NewProject.StartDate = project.StartDate;
                NewProject.EndDate = project.EndDate;
                NewProject.CurrentStatus = project.CurrentStatus;
                NewProject.EnabledFlag = project.EnabledFlag;
                NewProject.ClientId = project.ClientId;

                //dbActivityMapp
                if (project.RemoveActivity!=null)
                    if(project.RemoveActivity.Count>0)
                    {
                        foreach(var a in project.RemoveActivity)
                        {
                            NewProject.ActivityMapping.Where(X => X.ActivityId == a.ActivityId).
                                ToList().ForEach(X => X.ProjectId = 0);
                        }
                       

                    }
                this.SaveChanges();

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> AddActivity(List<ActivityMaster> viewModel)
        {
            try
            {
                foreach (var proj in viewModel)
                {
                    if (!await _context.Set<ActivityDetails>().Where(X => X.ActivityName.Trim() == proj.ActivityName.Trim().ToLower()).AnyAsync())
                    {

                        //var activty = new List<ActivityMapping>();


                        ActivityDetails activity = new ActivityDetails
                        {
                            //ActivityId=proj.ActivityId,
                            ActivityName = proj.ActivityName,
                            ActivityDescription = proj.ActivityDescription,
                            EnabledFlag = "true"

                        };

                        dbActivity.Add(activity);
                        this.SaveChanges();

                        return (await _context.Set<ActivityDetails>().Where(X => X.ActivityName.Trim() == proj.ActivityName.Trim().ToLower()).Select(X => X.ActivityId).FirstOrDefaultAsync());
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteActivity(List<ActivityMaster> viewModel)
        {
            try
            {
                var activity = new List<ActivityDetails>();
                foreach (var a in viewModel)
                {
                    //a.EnabledFlag = "false";
                    activity.Add(new ActivityDetails { ActivityId = a.ActivityId, EnabledFlag="false" });
                    
                }
                
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ProjectViewModel>> GetprojectListbyEmployeeID(string EmpID)
        {
            List<ProjectViewModel> rsp = new List<ProjectViewModel>();
            try
            {
                await Task.Run(() =>
                {
                    IQueryable<ProjectMapping> result = (IQueryable<ProjectMapping>)_context.Set<ProjectMapping>().
                                                        Where(X => X.EmployeeId == EmpID && X.Active == true).Include(c => c.Project).
                                                        ThenInclude(Z => Z.ActivityMapping).ThenInclude(y => y.Activity);

                    if (result.Any())
                    {
                        foreach (var i in result.Select(X => X.Project).Distinct())
                        {
                            var activities = i.ActivityMapping.Select(
                                X => new ProjectActivities
                                {
                                    ActivityDescription = X.Activity.ActivityDescription,
                                    ActivityId = X.ActivityId,
                                    ActivityName = X.Activity.ActivityName,
                                    EnabledFlag = X.Activity.EnabledFlag
                                }


                                ).ToList();

                            rsp.Add(new ProjectViewModel
                            {
                                ClientId = i.ClientId,
                                CurrentStatus = i.CurrentStatus,
                                EnabledFlag = i.EnabledFlag,
                                EndDate = i.EndDate,
                                ProjectId = i.ProjectId,
                                ProjectDescription = i.ProjectDescription,
                                ProjectName = i.ProjectName,
                                SredProject = i.SredProject,
                                StartDate = i.StartDate,
                                Activities = activities


                            });


                        }


                    }
                });
                return rsp.Count > 0 ? rsp : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool DeleteProject(List<ProjectViewModel> project)
        {
            try
            {
                List<ProjectDetails> details = new List<ProjectDetails>();
                ProjectDetails project1 = new ProjectDetails();
                foreach (var proj in project)
                {

                    //project1 = this.dbSet.Where(X => X.ProjectName == proj.ProjectName).FirstOrDefault();
                    bool TimesheetDetailForProjectPresent = timesheetDetail.GetTimesheetDetailsForProject(proj.ProjectId);
                    if (TimesheetDetailForProjectPresent)
                    {
                        return false;
                    }                 
                }
                foreach (var proj in project)
                {
                    var projectMapp = new List<ProjectMapping>();
                    project1 = this.dbSet.Where(X => X.ProjectId == proj.ProjectId).FirstOrDefault();
                    var activityMapp = new List<ActivityMapping>();
                    //foreach (var a in proj.Activities)
                    //{
                    //    a.EnabledFlag = "false";
                    //    var activitymapp = this.dbActivityMapp.Where(X => X.ProjectId == a.Pro).fi;//.Add(new ActivityMapping { ActivityId = a.ActivityId });

                    //}
                    activityMapp = this.dbActivityMapp.Where(X => X.ProjectId == proj.ProjectId).ToList();
                    projectMapp = this.dbSetProjectMapp.Where(X => X.ProjectId == proj.ProjectId).ToList();
                    ProjectDetails eachItem = this.GetById(project1.ProjectId);//new ProjectDetails() { ProjectId = proj.ProjectId };
                    
                    //ProjectMapping project1=_context.Add()
                    eachItem.ActivityMapping = activityMapp;
                    eachItem.ProjectMapping = projectMapp;
                    eachItem.EnabledFlag = "FALSE";
                       // this.Add(eachItem);
                        this.SaveChanges();
                    
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ProjectListViewModel>> GetAllProject()
        {
            List<ProjectListViewModel> rsp = new List<ProjectListViewModel>();
            try
            {
                await Task.Run(() =>
                {
                    rsp = (_context.Set<ProjectDetails>().Select(

                       X => new ProjectListViewModel
                       {
                           ProjectId = X.ProjectId,
                           ProjectName = X.ProjectName

                       }
                       )).ToList();
                });
                return rsp.Count > 0 ? rsp : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProjectViewModel> GetprojectDetailsByID(int ProjectID)
        {
            List<ProjectViewModel> rsp = new List<ProjectViewModel>();
            try
            {
                await Task.Run(() =>
                {
                    IQueryable<ProjectMapping> result = (IQueryable<ProjectMapping>)_context.Set<ProjectMapping>().
                                                        Where(X => X.ProjectId == ProjectID && X.Active == true).Include(c => c.Project).
                                                        ThenInclude(Z => Z.ActivityMapping).ThenInclude(y => y.Activity);

                    if (result.Any())
                    {
                        foreach (var i in result.Select(X => X.Project).Distinct())
                        {
                            var activities = i.ActivityMapping.Select(
                                X => new ProjectActivities
                                {
                                    ActivityDescription = X.Activity.ActivityDescription,
                                    ActivityId = X.ActivityId,
                                    ActivityName = X.Activity.ActivityName,
                                    EnabledFlag = X.Activity.EnabledFlag
                                }


                                ).ToList();

                            rsp.Add(new ProjectViewModel
                            {
                                ClientId = i.ClientId,
                                CurrentStatus = i.CurrentStatus,
                                EnabledFlag = i.EnabledFlag,
                                EndDate = i.EndDate,
                                ProjectId = i.ProjectId,
                                ProjectDescription = i.ProjectDescription,
                                ProjectName = i.ProjectName,
                                SredProject = i.SredProject,
                                StartDate = i.StartDate,
                                Activities = activities


                            });


                        }


                    }
                });
                return rsp.Count > 0 ? rsp[0] : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<ProjectListViewModel>> GetProjectList(string EmployeeID , string ProjectName)
        {
            List<ProjectListViewModel> rsp = new List<ProjectListViewModel>();
            try
            {
                await Task.Run(() =>
                {
                    if (string.IsNullOrEmpty(ProjectName))
                    {
                        rsp = (_context.Set<ProjectDetails>().Where(y => y.ClientId != null).Include(d => d.ProjectMapping).Include(X => X.Client).Select(

                           X => new ProjectListViewModel
                           {
                               ProjectId = X.ProjectId,
                               ProjectName = X.ProjectName,
                               ClientId = X.ClientId,
                               CurrentStatus = X.CurrentStatus,
                               EnabledFlag = X.EnabledFlag,
                               ProjectDescription = X.ProjectDescription,
                               SredProject = X.SredProject,
                               StartDate = X.StartDate,
                               EndDate = X.EndDate,
                               clientName = X.Client.ClientName,
                               EmployeeList = X.ProjectMapping.Where(a => a.Active == true && a.Employee.SupervisorFlag == "N").Select(u => u.Employee.FirstName + " " + u.Employee.LastName).ToList(),
                               SupervisorList = X.ProjectMapping.Where(a => a.Active == true && a.Employee.SupervisorFlag == "Y").Select(u => u.Employee.FirstName + " " + u.Employee.LastName).ToList()

                           }
                           )).ToList();
                    }
                    else
                    {
                        rsp = (_context.Set<ProjectDetails>().Where( y=>y.ProjectName.Trim().ToLower().Contains(ProjectName.Trim().ToLower())
                        ).Include(d => d.ProjectMapping).Include(X => X.Client).Select(

                         X => new ProjectListViewModel
                         {
                             ProjectId = X.ProjectId,
                             ProjectName = X.ProjectName,
                             ClientId = X.ClientId,
                             CurrentStatus = X.CurrentStatus,
                             EnabledFlag = X.EnabledFlag,
                             ProjectDescription = X.ProjectDescription,
                             SredProject = X.SredProject,
                             StartDate = X.StartDate,
                             EndDate = X.EndDate,
                             clientName = X.Client.ClientName,
                             EmployeeList = X.ProjectMapping.Where(a => a.Active == true && a.Employee.SupervisorFlag == "N").Select(u => u.Employee.FirstName + " " + u.Employee.LastName).ToList(),
                             SupervisorList = X.ProjectMapping.Where(a => a.Active == true && a.Employee.SupervisorFlag == "Y").Select(u => u.Employee.FirstName + " " + u.Employee.LastName).ToList()

                         }
                         )).ToList();
                    }
                });
                return rsp.Count > 0 ? rsp : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}
