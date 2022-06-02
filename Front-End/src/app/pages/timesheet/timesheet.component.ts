import { Component, OnInit } from '@angular/core';
import { HttpService } from 'app/_services/http.service';
import { UrlService } from 'app/_services/url.service';
import * as moment from 'moment';



@Component({
  selector: 'app-timesheet',
  templateUrl: './timesheet.component.html',
  moduleId: module.id,
  styleUrls: ['./timesheet.component.scss']
})
export class TimesheetComponent implements OnInit {
  userDetails: any;
  startOfWeek: any;
  endOfWeek: any;
  timeSheetDetails: any[];
  public config = {
    id: 'timesheet',
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 1,
  };


  constructor(private _url: UrlService,
    private _http: HttpService) { }

  ngOnInit(): void {
    // this.userDetails = JSON.parse(localStorage.getItem('token'));
    // this.getEmployeeDetails();
    this.startOfWeek = moment().startOf('isoWeek').toDate();
    this.endOfWeek = moment().endOf('isoWeek').toDate();
    this.timeSheetDetails = [{
        "Date": "2022-05-18 00:00:00.000",
        "EmployeeId": "92S5000000423",
        "EmployeeName": "Emp1",
        "ProjectId": 10,
        "ProjectName": "Other",
        "ActivityId": 12,
        "ActivityName": "Break",
        "NumberOfHours": 0.5,
        "monday":{ "value":0.5},
        "tuesday":{"value":0.5},
        "wednesday":{"value":0.5},
        "thurday":{"value":0.5},
        "friday":{"value":0.5},
        "saturday":{"value":0.5},
        "sunday":{"value":0.5},
        "Remarks": null,
        "UniqueId": 4
      },{
        "Date": "2022-05-18 00:00:00.000",
        "EmployeeId": "92S5000000423",
        "EmployeeName": "Emp1",
        "ProjectId": 10,
        "ProjectName": "Other",
        "ActivityId": 12,
        "ActivityName": "Break",
        "NumberOfHours": 0.5,
        "monday":{ "value":0.5},
        "tuesday":{"value":0.5},
        "wednesday":{"value":0.5},
        "thurday":{"value":0.5},
        "friday":{"value":0.5},
        "saturday":{"value":0.5},
        "sunday":{"value":0.5},
        "Remarks": null,
        "UniqueId": 4
      },{
        "Date": "2022-05-18 00:00:00.000",
        "EmployeeId": "92S5000000423",
        "EmployeeName": "Emp1",
        "ProjectId": 10,
        "ProjectName": "Other",
        "ActivityId": 12,
        "ActivityName": "Break",
        "NumberOfHours": 0.5,
        "monday":{ "value":0.5},
        "tuesday":{"value":0.5},
        "wednesday":{"value":0.5},
        "thurday":{"value":0.5},
        "friday":{"value":0.5},
        "saturday":{"value":0.5},
        "sunday":{"value":0.5},
        "Remarks": null,
        "UniqueId": 4
      },{
        "Date": "2022-05-18 00:00:00.000",
        "EmployeeId": "92S5000000423",
        "EmployeeName": "Emp1",
        "ProjectId": 10,
        "ProjectName": "Other",
        "ActivityId": 12,
        "ActivityName": "Break",
        "NumberOfHours": 0.5,
        "monday":{ "value":0.5},
        "tuesday":{"value":0.5},
        "wednesday":{"value":0.5},
        "thurday":{"value":0.5},
        "friday":{"value":0.5},
        "saturday":{"value":0.5},
        "sunday":{"value":0.5},
        "Remarks": null,
        "UniqueId": 4
      },{
        "Date": "2022-05-18 00:00:00.000",
        "EmployeeId": "92S5000000423",
        "EmployeeName": "Emp1",
        "ProjectId": 10,
        "ProjectName": "Other",
        "ActivityId": 12,
        "ActivityName": "Break",
        "NumberOfHours": 0.5,
        "monday":{ "value":0.5},
        "tuesday":{"value":0.5},
        "wednesday":{"value":0.5},
        "thurday":{"value":0.5},
        "friday":{"value":0.5},
        "saturday":{"value":0.5},
        "sunday":{"value":0.5},
        "Remarks": null,
        "UniqueId": 4
      },
      
    ]
    console.log("this.timeSheetDetails",this.timeSheetDetails);
  }
  private getEmployeeDetails(){

    this._http.get(`${this._url.login.getEmployeeDetails}/${this.userDetails.userId}`).subscribe(
      {
        next(res) {
          //localStorage.setItem('user',JSON.stringify(res.data));
        }
      }
    )
  }

  public pageChanged(event) {
    this.config.currentPage = event;
    //this.();
  }

}
