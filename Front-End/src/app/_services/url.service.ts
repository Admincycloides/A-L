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
    supervisorDecision        : 'api/Timesheet/supervisorDecision'
  }
  public Employee = {
    //getEmployeeDetails    : 'api/Employee/getEmployeeDetails',
    getSupervisorDetails  : 'api/EmployeeDetails/GetSupervisorDetails'
  }
  public project = {
    getprojectListbyEmployeeID :'api/Project/GetprojectListbyEmployeeID'
  }
}
