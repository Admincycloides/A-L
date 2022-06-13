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
  dropdownEmployeeSettings: IDropdownSettings = {};
  projectList: any[];
  employeeList: any[];
  maxPickerDateTo : any;
  minPickerDateTo : any;
  startDate: any;
  endDate: any;
  selectedProjectList = [];
  selectedEmployeeList = [];
  reportList :any[];
  allDates :any[];
  totalHours: any[]
  // maxPickerDateFrom : any;
  // minPickerDateFrom : any;

  constructor(private _url: UrlService,
    private _http: HttpService,
    private toast: ToastrService) { }

  ngOnInit(): void {
    this.dropdownSettings = {
      singleSelection: false,
      idField: 'projectId',
      textField: 'projectName',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 3,
      allowSearchFilter: true,
    };
    this.dropdownEmployeeSettings = {
      singleSelection: false,
      idField: 'employeeId',
      textField: 'employeeName',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 3,
      allowSearchFilter: true,
    }
    this.getProjectList();
    this.getEmployeeList();




    // this.reportList = [
    //   {
    //     "projectName": 'prjc 1',
    //     "employeeName": 'emp 1',
    //     "timeSpent": [
    //       {
    //         "Date": "2022-06-06T00:00:00",
    //         "numberOfHours": 2,
    //         "UniqueId": 0
    //       },
    //       {
    //         "Date": "2022-06-07T00:00:00",
    //         "numberOfHours": 0,
    //         "UniqueId": 0
    //       },
    //       {
    //         "Date": "2022-06-08T00:00:00",
    //         "numberOfHours": 0,
    //         "UniqueId": 0
    //       },
    //       {
    //         "Date": "2022-06-09T00:00:00",
    //         "numberOfHours": 0,
    //         "UniqueId": 0
    //       },{
    //         "Date": "2022-06-010T00:00:00",
    //         "numberOfHours": 0,
    //         "UniqueId": 0
    //       },
    //       {
    //         "Date": "2022-06-11T00:00:00",
    //         "numberOfHours": 0,
    //         "UniqueId": 0
    //       },
    //       {
    //         "Date": "2022-06-01200:00:00",
    //         "numberOfHours": 0,
    //         "UniqueId": 0
    //       },
    //       {
    //         "Date": "2022-06-13T00:00:00",
    //         "numberOfHours": 0,
    //         "UniqueId": 0
    //       },{
    //         "Date": "2022-06-14T00:00:00",
    //         "numberOfHours": 0,
    //         "UniqueId": 0
    //       },
    //       {
    //         "Date": "2022-06-15T00:00:00",
    //         "numberOfHours": 0,
    //         "UniqueId": 0
    //       },
    //       {
    //         "Date": "2022-06-16T00:00:00",
    //         "numberOfHours": 0,
    //         "UniqueId": 0
    //       },
    //       {
    //         "Date": "2022-06-17T00:00:00",
    //         "numberOfHours": 0,
    //         "UniqueId": 0
    //       },{
    //         "Date": "2022-06-18T00:00:00",
    //         "numberOfHours": 0,
    //         "UniqueId": 0
    //       },
    //       {
    //         "Date": "2022-06-19T00:00:00",
    //         "numberOfHours": 0,
    //         "UniqueId": 0
    //       },
    //       {
    //         "Date": "2022-06-20T00:00:00",
    //         "numberOfHours": 2,
    //         "UniqueId": 0
    //       },
    //       {
    //         "Date": "2022-06-21T00:00:00",
    //         "numberOfHours": 4,
    //         "UniqueId": 0
    //       }
    //     ]
    //   },
    //   ]


  }
  private getProjectList(){
    //this.projectList = ['Project 1','Project 2','Project 2','Project 3','Project 4'];
    const url =`${this._url.project.getProjectList}`
    this._http.get(url).subscribe(
      {
        next:(res:any)=> {
          this.projectList = res.data;
          //console.log(this.projectList)
        },
        error:(msg) =>{
        }
      })
  }
  private getEmployeeList(){
    //this.employeeList = ['Employee 1','Employee 2','Employee 2','Employee 3','Employee 4'];
    const url =`${this._url.Employee.getAllEmployeeList}`
    this._http.get(url).subscribe(
      {
        next:(res:any)=> {
          this.employeeList = res.data;
        },
        error:(msg) =>{
        }
      })
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
    while(moment(currentDate).format("YYYY-MM-DD 00:00:00.000")<=moment(stopDate).format("YYYY-MM-DD 00:00:00.000")){
      //dateArray.push(moment(currentDate).format("MMMM-DD"));
      dateArray.push(currentDate);
      currentDate = moment(currentDate).add(1,'days');
    }
    return dateArray;
  }

  //To get total hour spent to a project by a employee
  public getTotalHours(item:any){
    let total = 0;
    item.timeSpent.forEach(el => {
      total = total + el.numberOfHours
    });
    return total;
  }
  //To check whether all 4 fields are selected or not
  private setStatus(){

    if(this.selectedEmployeeList?.length && this.selectedProjectList?.length && this.startDate && this.endDate) return true;
    else return false;
  }

  //To Genearet Report
  public onGenerateReport(){
    if(this.setStatus()){
      if(this.endDate.getFullYear() - this.startDate.getFullYear() <= 1){
        const pList = [];
        const eList = [];
        this.allDates = this.dateFormatter(this.startDate,this.endDate);
        console.log(this.allDates);
        const sDate = moment(this.startDate).format("YYYY-MM-DD 00:00:00.000")
        const eDate = moment(this.endDate).format("YYYY-MM-DD 00:00:00.000");
        this.selectedProjectList.forEach((item)=>{pList.push(item.projectId)});
        this.selectedEmployeeList.forEach((item)=>{eList.push(item.employeeId)});

        // console.log("sDate",sDate);
        // console.log("Edate",eDate)
        // console.log("pIds",pList);
        // console.log("EIds",eList);

        const body = {
          projectIds:pList,
          employeeId:eList,
          fromDate:sDate,
          toDate: eDate
        };
        const url = `${this._url.timesheet.getTimesheetReport}`;
        this._http.post(url,body).subscribe({
            next:(res:any)=>{
              console.log(res);
              this.reportList = res.reportViewModels;
              this.totalHours = res.reportDayWiseTotals;
          }
        })

      }else{  
        this.toast.error("Please select date with in one year!!")
      }

    }else{
      this.toast.error("All Filds Are Mandatory!!")
    }
  }

}
