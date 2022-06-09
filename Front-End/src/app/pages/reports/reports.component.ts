import { Component, OnInit } from '@angular/core';
import { HttpService } from 'app/_services/http.service';
import { UrlService } from 'app/_services/url.service';
import * as moment from 'moment';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss']
})
export class ReportsComponent implements OnInit {
  dropdownSettings: IDropdownSettings = {};
  projectList: any[];
  employeeList: any[];
  maxPickerDateTo : any;
  minPickerDateTo : any;
  startDate: any;
  endDate: any;
  selectedProjectList = [];
  selectedEmployeeList = [];
  reportList :any[];
  allDates :any[]
  // maxPickerDateFrom : any;
  // minPickerDateFrom : any;

  constructor(private _url: UrlService,
    private _http: HttpService,
    private toast: ToastrService) { }

  ngOnInit(): void {
    this.dropdownSettings = {
      singleSelection: false,
      idField: 'id',
      textField: 'name',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 3,
      allowSearchFilter: true,
    };
    this.getProjectList();
    this.getEmployeeList();




    this.reportList = [
      {
        "ProjectName": 'prjc 1',
        "EmployeeName": 'emp 1',
        "TimeSpent": [
          {
            "Date": "2022-06-06T00:00:00",
            "NumberOfHours": 0,
            "UniqueId": 0
          },
          {
            "Date": "2022-06-07T00:00:00",
            "NumberOfHours": 0,
            "UniqueId": 0
          },
          {
            "Date": "2022-06-08T00:00:00",
            "NumberOfHours": 0,
            "UniqueId": 0
          },
          {
            "Date": "2022-06-09T00:00:00",
            "NumberOfHours": 0,
            "UniqueId": 0
          },{
            "Date": "2022-06-010T00:00:00",
            "NumberOfHours": 0,
            "UniqueId": 0
          },
          {
            "Date": "2022-06-11T00:00:00",
            "NumberOfHours": 0,
            "UniqueId": 0
          },
          {
            "Date": "2022-06-01200:00:00",
            "NumberOfHours": 0,
            "UniqueId": 0
          },
          {
            "Date": "2022-06-13T00:00:00",
            "NumberOfHours": 0,
            "UniqueId": 0
          },{
            "Date": "2022-06-14T00:00:00",
            "NumberOfHours": 0,
            "UniqueId": 0
          },
          {
            "Date": "2022-06-15T00:00:00",
            "NumberOfHours": 0,
            "UniqueId": 0
          },
          {
            "Date": "2022-06-08T00:00:00",
            "NumberOfHours": 0,
            "UniqueId": 0
          },
          {
            "Date": "2022-06-09T00:00:00",
            "NumberOfHours": 0,
            "UniqueId": 0
          },{
            "Date": "2022-06-06T00:00:00",
            "NumberOfHours": 0,
            "UniqueId": 0
          },
          {
            "Date": "2022-06-07T00:00:00",
            "NumberOfHours": 0,
            "UniqueId": 0
          },
          {
            "Date": "2022-06-08T00:00:00",
            "NumberOfHours": 0,
            "UniqueId": 0
          },
          {
            "Date": "2022-06-09T00:00:00",
            "NumberOfHours": 0,
            "UniqueId": 0
          }
        ]
      },
      ]


  }
  private getProjectList(){
    this.projectList = ['Project 1','Project 2','Project 2','Project 3','Project 4'];
    // const url =`${this._url.project.getProjectList}`
    // this._http.get(url).subscribe(
    //   {
    //     next:(res:any)=> {
    //       this.projectList = res.data;
    //     },
    //     error:(msg) =>{
    //     }
    //   })
  }
  private getEmployeeList(){
    this.employeeList = ['Employee 1','Employee 2','Employee 2','Employee 3','Employee 4'];
    // const url =`${this._url.project.getProjectList}`
    // this._http.get(url).subscribe(
    //   {
    //     next:(res:any)=> {
    //       this.projectList = res.data;
    //     },
    //     error:(msg) =>{
    //     }
    //   })
  }
  public onProjectEmployeeDeSelect(item:any,value:any){
    if(value === 'project'){
      this.selectedProjectList.splice(this.selectedProjectList.indexOf(item),1);
    }else{
      this.selectedEmployeeList.splice(this.selectedEmployeeList.indexOf(item),1);
    }
    console.log(this.selectedProjectList);
  }
  public onProjectEmployeeSelect(item:any,value:any){
    if(value === 'project'){
      this.selectedProjectList.push(item);
    }else{
      this.selectedEmployeeList.push(item)
    }
  }
  public onProjectEmployeeSelectAll(item:any,value:any){
    if(value === 'project') this.selectedEmployeeList = this.employeeList;
    if(value==='employee') this.selectedProjectList = this.projectList;
  }
  public onProjectEmployeeDeSelectAll(item:any,value:any){
    if(value === 'project') this.selectedEmployeeList = [];
    if(value==='employee') this.selectedProjectList = [];
  }
  public dateChange(event:any,value:any){
    if(value === 'from'){
      this.maxPickerDateTo = {
        year: event.year+1,
        month:event.month,
        day: event.day
      }
      this.minPickerDateTo = event;
      const date = new Date();
      date.setFullYear(event.year);
      date.setMonth(event.month - 1);
      date.setDate(event.day);
      this.startDate = date;
      // this.minPickerDateFrom = {}
      // this.maxPickerDateFrom ={};
    }else{
      const date = new Date();
      date.setFullYear(event.year);
      date.setMonth(event.month - 1);
      date.setDate(event.day);
      this.endDate = date
      // this.minPickerDateFrom = {
      //   year: event.year-1,
      //   month:event.month,
      //   day: event.day
      // }
      // this.maxPickerDateFrom = event;
      // this.minPickerDateTo = {}
      // this.maxPickerDateTo ={};
    }
  }
  private dateFormatter(start:any,end:any){
    var dateArray = [];
    var currentDate = moment(start);
    var stopDate = moment(end);
    while(currentDate<=stopDate){
      //dateArray.push(moment(currentDate).format("MMMM-DD"));
      dateArray.push(currentDate);
      currentDate = moment(currentDate).add(1,'days');
    }
    return dateArray;
  }

  public onGenerateReport(){
    if(this.endDate.getFullYear() - this.startDate.getFullYear() <= 1){
      
      this.allDates = this.dateFormatter(this.startDate,this.endDate)
      //console.log(this.endDate.getFullYear() - this.startDate.getFullYear());
      console.log("all dated",this.allDates);






    }else{
      console.log(this.startDate.year - this.endDate )
      this.toast.error("Please select date with in one year!!")
    }

  }

}
