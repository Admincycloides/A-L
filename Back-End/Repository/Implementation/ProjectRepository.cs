using AnL.Models;
using AnL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AnL.ViewModel;
using System.Linq;
using System.Threading.Tasks;
using System;
using AnL.Helpers.AuditTrail;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AnL.Repository.Implementation
{
    public class ProjectRepository : Repository<ProjectDetails>, IProject
    {
        private DbContext _context;
        private readonly IUnitOfWork _UOW;
        private TimesheetDetailRepository timesheetDetail;
        private AuditRepository audit;
        //private readonly MopDbContext Db;

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
            audit = new AuditRepository(context, UOW);

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
        

        public async Task<object> AddProject(List<ProjectViewModel> viewModel,string Userid)
        {
            try
            {
                var ids = new List<int>();
                List<ProjectDetails> projects = new List<ProjectDetails>();
                foreach (var proj in viewModel)
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
                        var employeeMapp = new List<ProjectMapping>();
                        
                        this.Add(Project);
                        audit.AddAuditLogs(Userid);
                        this.SaveChanges();

                        foreach (var a in proj.EmployeeID)
                        {
                            employeeMapp.Add(new ProjectMapping { 
                                ProjectId = Project.ProjectId, 
                                EmployeeId = a, 
                                Active=true,
                                LastUpdatedBy = Userid,
                                LastUpdate= DateTime.Now
                            });
                        }
                        Project.ProjectMapping = employeeMapp;
                        //this.dbSetProjectMapp.AddRange(employeeMapp);
                        audit.AddAuditLogs(Userid);
                        this.SaveChanges();
                        projects.Add(new ProjectDetails { ProjectId=Project.ProjectId });

                        //foreach (var a in proj.Activities)
                        //{
                        //    activty.Add(new ActivityMapping { ActivityId = a.ActivityId, ProjectId= Project.ProjectId });
                        //}
                        //Project.ActivityMapping = activty;
                        //this.SaveChanges();

                        var result = await _context.Set<ProjectDetails>().Where(X => X.ProjectName.Trim() == proj.ProjectName.Trim().ToLower()).Select(X => X.ProjectId).FirstOrDefaultAsync();
                        
                    }
                    else
                    {
                        return null;
                    }
                }
                
                return true;
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
                   rsp = (_context.Set<ActivityDetails>().Where(x=> (x.EnabledFlag).ToLower()=="true").Select(

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
       
        public async Task<object> EditProjectDetails(EditProjectView project, string userid)
        {
            try
            {
                var NewProject = this.dbSet.Where(X => X.ProjectId == project.ProjectId).FirstOrDefault();
                NewProject.ProjectDescription = project.ProjectDescription;
                NewProject.ProjectName = project.ProjectName;
                NewProject.StartDate = project.StartDate;
                NewProject.EndDate = project.EndDate;
                NewProject.CurrentStatus = project.CurrentStatus;
                NewProject.EnabledFlag = project.EnabledFlag;
                NewProject.ClientId = project.ClientId;
                audit.AddAuditLogs(userid);
                this.SaveChanges();

                return null;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<object> EditProjectActive(EditProjectView project,string userid)
        {
            try
            {
                if (project.NewActivity != null)
                    if (project.NewActivity.Count > 0)
                    {
                        var activty = new List<ActivityMapping>();
                        foreach (var a in project.NewActivity)
                        {
                            activty.Add(new ActivityMapping { ActivityId = a.ActivityId, ProjectId = project.ProjectId , IsActive=true});
                        }
                        this.dbActivityMapp.AddRange(activty);
                        this.SaveChanges();
                    }


                var NewProject = this.dbSet.Where(X => X.ProjectId == project.ProjectId).Include(y=>y.ActivityMapping).FirstOrDefault();
                
                //dbActivityMapp
                if (project.RemoveActivity != null)
                    if (project.RemoveActivity.Count > 0)
                    {
                        foreach (var a in project.RemoveActivity)
                        {
                            NewProject.ActivityMapping.Where(X => X.ActivityId == a.ActivityId).
                                ToList().ForEach(X => X.IsActive = false);
                        }


                    }
                audit.AddAuditLogs(userid);
                this.SaveChanges();
              

                return null;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<object> EditProject(EditProjectView project,string userid)
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
                audit.AddAuditLogs(userid);
                this.SaveChanges();

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<object> EditActivity(List<ActivityMaster> viewModel,string userid)
        {
            try
            {
                foreach (var proj in viewModel)
                {
                    var activityData = await _context.Set<ActivityDetails>().
                        Where(X => X.ActivityId==proj.ActivityId).FirstOrDefaultAsync();
                    if(activityData!=null)
                    {
                        if(activityData.ActivityName.ToLower() != proj.ActivityName.Trim().ToLower())
                        {
                            if(!_context.Set<ActivityDetails>().
                                Where(X => X.ActivityName.ToLower() == proj.ActivityName.Trim().ToLower()).Any())
                            {
                                activityData.ActivityName = proj.ActivityName;

                            }
                            else
                            {
                                return "Axtivity Name already Exist.";
                            }
                        }
                        activityData.ActivityDescription = proj.ActivityDescription;
                        
                    }
                    audit.AddAuditLogs(userid);
                    this.SaveChanges();
                    
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        public async Task<object> AddActivity(List<ProjectActivityMap> viewModel,string userid)
        {
            try
            {
                //IMopDbContext Db1 = new MopDbContext();
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
                        audit.AddAuditLogs(userid);
                        this.SaveChanges();

                        dbActivityMapp.Add(new ActivityMapping
                        {
                            ActivityId=activity.ActivityId,
                            ProjectId= proj.ProjectId,
                            IsActive=true
                        });
                        
                        
                        
                        audit.AddAuditLogs(userid);
                        this.SaveChanges();


                        //var a = await _context.Set<ActivityDetails>().Where(X => X.ActivityName.Trim() == proj.ActivityName.Trim().ToLower()).Select(X => X.ActivityId).FirstOrDefaultAsync();
                       // return (a);
                    }
                    else
                    {
                        return null;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public bool DeleteActivity(int activityID,string userid)
        {
            
            try
            {
                var activity = new ActivityDetails();
                List<ActivityMapping> activityMapp = new List<ActivityMapping>();
                activity = this.dbActivity.Find(activityID);
                
                foreach(var a in activity.ActivityMapping)
                {
                    //project1 = this.dbSet.Find(a.ProjectId);
                    bool TimesheetDetailForProjectPresent = timesheetDetail.GetTimesheetDetailsForProject(a.ProjectId);
                    if (TimesheetDetailForProjectPresent)
                    {
                        return false;
                    }
                }
                foreach (var a in activity.ActivityMapping)
                {
                    a.IsActive = false;
                }
                //activity = this.dbActivity.Find(activityID);
                activity.EnabledFlag = "False";
                audit.AddAuditLogs(userid);
                this.SaveChanges();
                return true;
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

        public bool DeleteProject(int project,string userid)
        {
            try
            {
                List<ProjectDetails> details = new List<ProjectDetails>();
                ProjectDetails project1 = new ProjectDetails();
               // foreach (var proj in project)
                {

                    //project1 = this.dbSet.Where(X => X.ProjectName == proj.ProjectName).FirstOrDefault();
                    bool TimesheetDetailForProjectPresent = timesheetDetail.GetTimesheetDetailsForProject(project);
                    if (TimesheetDetailForProjectPresent)
                    {
                        return false;
                    }                 
                }
                //foreach (var proj in project)
                {
                    var projectMapp = new List<ProjectMapping>();
                    project1 = this.dbSet.Where(X => X.ProjectId == project).FirstOrDefault();
                    var activityMapp = new List<ActivityMapping>();
                    //foreach (var a in proj.Activities)
                    //{
                    //    a.EnabledFlag = "false";
                    //    var activitymapp = this.dbActivityMapp.Where(X => X.ProjectId == a.Pro).fi;//.Add(new ActivityMapping { ActivityId = a.ActivityId });

                    //}
                    activityMapp = this.dbActivityMapp.Where(X => X.ProjectId == project).ToList();
                    projectMapp = this.dbSetProjectMapp.Where(X => X.ProjectId == project).ToList();
                    ProjectDetails eachItem = this.GetById(project1.ProjectId);//new ProjectDetails() { ProjectId = proj.ProjectId };
                    
                    //ProjectMapping project1=_context.Add()
                    eachItem.ActivityMapping = activityMapp;
                    eachItem.ProjectMapping = projectMapp;
                    eachItem.EnabledFlag = "FALSE";
                    // this.Add(eachItem);
                    audit.AddAuditLogs(userid);
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

        public async Task<List<ProjectListingViewModel>> GetProjectList(string EmployeeID , string ProjectName)
        {
            List<ProjectListingViewModel> rsp = new List<ProjectListingViewModel>();
            try
            {
                await Task.Run(() =>
                {
                    if (string.IsNullOrEmpty(ProjectName))
                    {
                        rsp = (_context.Set<ProjectDetails>().Where(y => y.ClientId != null).Where(y => y.EnabledFlag.ToLower() == "true").Include(d => d.ProjectMapping).Include(X => X.Client).Select(

                           X => new ProjectListingViewModel
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
                               EmployeeList = X.ProjectMapping.Where(a => a.Active == true && a.Employee.SupervisorFlag == "N").Select(u => new EmployeeListViewModel {EmployeeId=u.EmployeeId,EmployeeName=String.Concat( u.Employee.FirstName + " " + u.Employee.LastName )}).ToList(),
                               SupervisorList = X.ProjectMapping.Where(a => a.Active == true && a.Employee.SupervisorFlag == "Y").Select(u => new EmployeeListViewModel { EmployeeId=u.EmployeeId, EmployeeName = String.Concat(u.Employee.FirstName + " " + u.Employee.LastName) }).ToList()

                           }
                           )).ToList();
                    }
                    else
                    {
                        rsp = (_context.Set<ProjectDetails>().Where( y=>y.ProjectName.Trim().ToLower().Contains(ProjectName.Trim().ToLower())
                        ).Where(y => y.EnabledFlag.ToLower() == "true").Include(d => d.ProjectMapping).Include(X => X.Client).Select(

                         X => new ProjectListingViewModel
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
                             EmployeeList = X.ProjectMapping.Where(a => a.Active == true && a.Employee.SupervisorFlag == "N").Select(u => new EmployeeListViewModel { EmployeeId = u.EmployeeId, EmployeeName = String.Concat(u.Employee.FirstName + " " + u.Employee.LastName) }).ToList(),
                             SupervisorList = X.ProjectMapping.Where(a => a.Active == true && a.Employee.SupervisorFlag == "Y").Select(u => new EmployeeListViewModel { EmployeeId = u.EmployeeId, EmployeeName = String.Concat(u.Employee.FirstName + " " + u.Employee.LastName) }).ToList()

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
