using System;
using System.Collections.Generic;

namespace AnL.ViewModel
{
    public class ProjectActivities
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string ActivityDescription { get; set; }
        public string EnabledFlag { get; set; }
    }
    public class ProjectViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public int ClientId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CurrentStatus { get; set; }
        public string SredProject { get; set; }
        public string EnabledFlag { get; set; }
        public List<ProjectActivities> Activities{ get; set; }
    }

    public class EditProjectView
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public int ClientId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CurrentStatus { get; set; }
        public string SredProject { get; set; }
        public string EnabledFlag { get; set; }

        public List<ProjectActivities> NewActivity { get; set; }
        public List<ProjectActivities> RemoveActivity { get; set; }

    }


    public class ProjectListViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
