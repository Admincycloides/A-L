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
  
  timeSheetDetails:any[]
  currentWeek :any[];
  public config = {
    id: 'timesheet',
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 1,
  };
  //timeSheetDetailsArray = new Array(11);
  timeSheetDetailsArray =[];
  model;
  weekShow:any;


  constructor(private _url: UrlService,
    private _http: HttpService) { }

  ngOnInit(): void {
    // this.userDetails = JSON.parse(localStorage.getItem('token'));
    // this.getEmployeeDetails();
    this.startOfWeek = moment().startOf('isoWeek').toDate();
    this.endOfWeek = moment().endOf('isoWeek').toDate();
    this.weekShow = moment(this.startOfWeek).format("MMMM-DD")+"-"+moment(this.endOfWeek).format("MMMM-DD");
    this.currentWeek = this.dateFormatter(moment(this.startOfWeek).format("YYYY-MM-DD"),moment(this.endOfWeek).format("YYYY-MM-DD"))
    this.timeSheetDetails= [
      {
        "Date": "2022-05-30 00:00:00.000",
        "EmployeeId": "92S5000000423",
        "EmployeeName": "Emp1",
        "ProjectId": 10,
        "ProjectName": "Other",
        "ActivityId": 12,
        "ActivityName": "Break",
        "NumberOfHours": 0.5,
        "Remarks": null,
        "Status": "In Progress",
        "LastUpdatedDate": "2022-05-18 00:00:00.000",
        "LastUpdatedBy": "Emp1",
        "UniqueId": 4
      },
      {
        "Date": "2022-06-03 00:00:00.000",
        "EmployeeId": "92S5000000423",
        "EmployeeName": "Emp1",
        "ProjectId": 10,
        "ProjectName": "Other",
        "ActivityId": 13,
        "ActivityName": "Lunch",
        "NumberOfHours": 0.5,
        "Remarks": "Flagging potato and tomato plots",
        "Status": "In Progress",
        "LastUpdatedDate": "2022-05-19 00:00:00.000",
        "LastUpdatedBy": "Emp1",
        "UniqueId": 5
      }
    ]
    this.getTimesheetDetails()
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
  // [
  //   [projectName,projectID,monday,tues,]
  //   [projectName,projectID,monday,tues,]
  //   [projectName,projectID,monday,tues,]
  // ]
  getTimesheetDetails(){
    this.timeSheetDetails.forEach(
      (item,index)=>{
        let rowArray =[];
        let itemIndex = this.currentWeek.indexOf(moment(item.Date).format("MMMM-DD"));
        rowArray.push(item.Status,item.ProjectName,item.ActivityName);
        while(rowArray.length < 10){
          if(rowArray.length != 10){
            if(rowArray.length == itemIndex+3){
              rowArray.push(item.NumberOfHours);
            }
            else{
              rowArray.push(0);
            }
          }
        }
        // rowArray.splice(itemIndex+2,0,item.NumberOfHours);
        // rowArray.splice(9,0,5);
        rowArray.push(item.NumberOfHours,item.Remarks);
         this.timeSheetDetailsArray.push(rowArray);
      }
      
    )
  }
  dateChange(event){
    const date = event.month +'-'+event.day+"-"+event.year
    this.startOfWeek = moment(date).startOf('isoWeek').toDate();
    this.endOfWeek = moment(date).endOf('isoWeek').toDate();
    this.model = "2022-12-5";
    this.weekShow = moment(this.startOfWeek).format("MMMM-DD")+"-"+moment(this.endOfWeek).format("MMMM-DD")
  }
  private dateFormatter(start:any,end:any){
    var dateArray = [];
    var currentDate = moment(start);
    var stopDate = moment(end);
    while(currentDate<=stopDate){
      dateArray.push(moment(currentDate).format("MMMM-DD"));
      currentDate = moment(currentDate).add(1,'days');
    }
    return dateArray;
  }
  onPreviousClick(){
    this.startOfWeek = moment(this.startOfWeek).subtract(1,'weeks');
    this.endOfWeek = moment(this.endOfWeek).subtract(1,'weeks');
    this.currentWeek = this.dateFormatter(moment(this.startOfWeek).format("YYYY-MM-DD"),moment(this.endOfWeek).format("YYYY-MM-DD"));
    this.weekShow = moment(this.startOfWeek).format("MMMM-DD")+"-"+moment(this.endOfWeek).format("MMMM-DD")
  }
  onNextClick(){
    this.startOfWeek = moment(this.startOfWeek).add(1,'weeks');
    this.endOfWeek = moment(this.endOfWeek).add(1,'weeks');
    this.currentWeek = this.dateFormatter(moment(this.startOfWeek).format("YYYY-MM-DD"),moment(this.endOfWeek).format("YYYY-MM-DD"));
    this.weekShow = moment(this.startOfWeek).format("MMMM-DD")+"-"+moment(this.endOfWeek).format("MMMM-DD")

  }

}
