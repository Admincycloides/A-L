import { Component, OnInit } from '@angular/core';
import { HttpService } from 'app/_services/http.service';
import { UrlService } from 'app/_services/url.service';
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
  }
  private getProjectList(){
    this.projectList = ['Project 1','Project 2','Project 2','Project 3','Project 4'];
    const url =`${this._url.project.getProjectList}`
    this._http.get(url).subscribe(
      {
        next:(res:any)=> {
          this.projectList = res.data;
        },
        error:(msg) =>{
        }
      })
  }
  private getEmployeeList(){
    this.employeeList = ['Employee 1','Employee 2','Employee 2','Employee 3','Employee 4'];
    const url =`${this._url.project.getProjectList}`
    this._http.get(url).subscribe(
      {
        next:(res:any)=> {
          this.projectList = res.data;
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
      this.startDate = event;
      this.maxPickerDateTo = {
        year: event.year+1,
        month:event.month,
        day: event.day
      }
      this.minPickerDateTo = event;
      // this.minPickerDateFrom = {}
      // this.maxPickerDateFrom ={};
    }else{
      this.endDate = event;
      // this.minPickerDateFrom = {
      //   year: event.year-1,
      //   month:event.month,
      //   day: event.day
      // }
      // this.maxPickerDateFrom = event;
      // this.minPickerDateTo = {}
      // this.maxPickerDateTo ={};
    }
    console.log("date",event);
  }

  public onGenerateReport(){
    if(this.startDate.year - this.endDate <=1){

    }else{
      this.toast.error("Please select date with in one year!!")
    }

  }

}
