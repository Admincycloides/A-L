import { Component, OnInit } from '@angular/core';
import { IDropdownSettings } from 'ng-multiselect-dropdown';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss']
})
export class ReportsComponent implements OnInit {
  dropdownSettings: IDropdownSettings = {};
  projectList: any[];
  employeeList: any[]

  constructor() { }

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
    this.projectList = ['Project 1','Project 2','Project 2','Project 3','Project 4']
  }
  public onProjectEmployeeDeSelect(item:any,value:any){
    console.log(item);

  }
  public onProjectEmployeeSelect(item:any,value:any){
    console.log(item);
  }
  public onProjectEmployeeSelectAll(item:any,value:any){
    console.log(item);
  }
  public onProjectEmployeeDeSelectAll(item:any,value:any){
    console.log(item);
  }

}
