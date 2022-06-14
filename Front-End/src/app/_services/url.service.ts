import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UrlService {

  constructor() { }
  getRootURL(): String {
    const URL = environment.apiConfig.server;
    return URL;
  }
  getAPIURL(): String {
    const URL = environment.apiConfig.api;
    return URL;
  }
  // api       = environment.apiConfig.api;
  // serverApi = environment.apiConfig.server;



  // // Login
  // generateOTP = this.api + '/GenerateOTP';
  // submitOTP   = this.api + '/SubmitOTP';

  // Login
  public user = {
    getOTP              : 'api/User/GenerateOTP',
    submitOTP           : 'api/User/SubmitOTP',
  }
  public timesheet = {
    getTimesheet              : 'api/Timesheet/GetDetails',
    deleteTimesheet           : 'api/Timesheet/DeleteTimesheet',
    addTimesheet              : 'api/Timesheet/AddTimesheetDetails',
    editTimesheet             : 'api/Timesheet/ModifyTimesheet',
    submitTimesheet           : 'api/Timesheet/SubmitTimesheetDetails',
    getReviewTimesheet        : 'api/Timesheet/GetReviewTimesheet',
    getReviewTimesheetDetails : 'api/Timesheet/GetReviewTimesheetDetails',
    supervisorDecision        : 'api/Timesheet/supervisorDecision',
    getTimesheetReport        : 'api/Timesheet/GetTimesheetReport'
  }
  public Employee = {
    //getEmployeeDetails    : 'api/Employee/getEmployeeDetails',
    getSupervisorDetails     : 'api/EmployeeDetails/GetSupervisorDetails',
    getAllEmployeeList       : 'api/EmployeeDetails/GetAllEmployeeList'
  }
  public project = {
    getprojectListbyEmployeeID :'api/Project/GetprojectListbyEmployeeID',
    getProjectList             : 'api/Project/GetAllProjectList',
    addProject : 'api/Project/AddProject',
    deleteProject : 'api/Project/DeleteProject',
    allocateResources : 'api/Project/allocateResources',
    getallprojectlist : 'api/Project/GetAllProjectList',
    getprojectlist: 'api/Project/GetProjectList',
    getclientlist : 'api/Project/GetClientList',
    editproject : 'api/Project/EditProject',
    getprojectdetailsByid:'api/Project/GetprojectDetailsByID?ProjectID=1'
  }

  public activity = {
    addActivity : 'api/Project/AddActivity',
    getActivityList : 'api/Project/GetActivityList',

  }
}
