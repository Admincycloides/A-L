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
                            activty.Add(new ActivityMapping { ActivityId = a.ActivityId });
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
                
                foreach (var proj in project)
                {
                    
                    bool TimesheetDetailForProjectPresent = timesheetDetail.GetTimesheetDetailsForProject(proj.ProjectId);
                    if (TimesheetDetailForProjectPresent)
                    {
                        return false;
                    }                 
                }
                foreach (var proj in project)
                {
                    //var projectMapp = new List<ProjectMapping>();
                    IQueryable<ProjectMapping> result = (IQueryable<ProjectMapping>)_context.Set<ProjectMapping>().
                                                         Include(c => c.Project).
                                                        ThenInclude(Z => Z.ActivityMapping).ThenInclude(y => y.Activity);
                    var activity = new List<ActivityMapping>();
                    foreach (var a in proj.Activities)
                    {
                        a.EnabledFlag = "false";
                        activity.Add(new ActivityMapping { ActivityId = a.ActivityId });
                        
                    }
                    
                    ProjectDetails eachItem = new ProjectDetails() { ProjectId = proj.ProjectId };
                    
                    //ProjectMapping project1=_context.Add()
                    eachItem.ActivityMapping = activity;
                    eachItem.EnabledFlag = "FALSE";
                        this.Modify(eachItem);
                        this.SaveChanges();
                    
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
