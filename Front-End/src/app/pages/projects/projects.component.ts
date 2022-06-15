import { Component, OnInit,NgModule } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HttpService } from 'app/_services/http.service';
import { UrlService } from 'app/_services/url.service';
import { IDropdownSettings } from "ng-multiselect-dropdown";
import { data } from 'jquery';
import { ToastrService } from 'ngx-toastr';
import * as moment from 'moment';
@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.scss']
})
export class ProjectsComponent implements OnInit {
  public text: string = 'Add Project';
  public index: any = '';
  searchTerm: any= '';
  userDetails: any;
  dropdownEmployeeSettings: IDropdownSettings = {};
  getActivityName:any;
  employeeList:any[];
  ProjectItems:any;
  selectedactivity:any;
  supervisorFlag: string;
  selectedEmployeeList:any;
  user:any;
  getclientdescription:any;
  
  public isSubmitted: boolean = false;
  projectGroup : FormGroup;
  TotalRow : number; 
  SelectedValue:any;
  public toggleButton: boolean = false;

  changeid(e)
  {
    this.SelectedValue= e.target.value;
    console.log(this.SelectedValue)
  }
  // public addProjects = [];

  config: any;
  collection = { count: 60, data: [] };

  constructor(private _url: UrlService,
    private _fb:FormBuilder,private _http: HttpService, private _toast:ToastrService
  ) { 
    this.projectGroup = this._fb.group({
      itemRows:this._fb.array([]),
    });

    this.config = {
      itemsPerPage: 5,
      currentPage: 1,
      totalItems: this.collection.count
    };
  }
  

  ngOnInit(): void {

    this.user = JSON.parse(localStorage.getItem('user'));
    console.log("macha",this.user);
    this.getListofProjects();
    this.getActivityLists();
    this.GetClientLists();
    this.getEmployeeList();
    this.dropdownEmployeeSettings = {
      singleSelection: false,
      idField: "employeeId",
      textField: "employeeName",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 3,
      allowSearchFilter: true,
    };
    this.toggleButton = true;
    // this.getProjectActivityDetails(),
  }

 
  // private getProjectActivityDetails(){

  //   const url = `${this._url.project.getprojectListbyEmployeeID}`
  //   this._http.get(url).subscribe({
  //     next:(res:any)=>{
  //       this.projectGroup['controls'].itemRows['controls'] = (res.data);}
  //     })
  //   }

  public changeText(){
    if (this.text === 'Add Project') {
      this.text = 'save';
    } else {
      this.text = 'Add Project';
    }
}


    private getActivityLists(){
      console.log("first call")
      const url = `${this._url.activity.getActivityList}`
      this._http.get(url).subscribe({next:(res:any)=>{
          this.getActivityName = res.data;
          console.log("data",res.data)}
        })
        }

    private GetClientLists(){
      const url = `${this._url.project.getclientlist}`
      this._http.get(url).subscribe({next:(res:any)=>{
        this.getclientdescription = res.data;
        console.log("client",res.data)}
      })
    }

    private getListofProjects(){
      const search = this.config.search;
      const url = `${this._url.project.getprojectlist}?empID=${this.user.employeeID}&ProjectName=${this.searchTerm}`;
      console.log(url)
      // this.itemRows.remove
      this._http.get(url).subscribe({
        next:(res:any)=>{
          // this.itemRows.setValue(res.data);
          var items = [];
          res.data.forEach(element => {
            this.addFieldValue();
              let i = {
                projectId:element.projectId,
                projectName:element.projectName,
                projectDescription:element.projectDescription,
                clientName:element.clientName,
                clientId:element.clientId,
                startDate:element.startDate != null?this.formatDate(element.startDate): '',
                endDate:element.endDate != null?this.formatDate(element.endDate):'',
                currentStatus:element.currentStatus,
                sredProject:element.sredProject,
                enabledFlag:element.enabledFlag,
                assignedTo:element.supervisorList != null ?element.supervisorList.join(', '):'',
              }
              items.push(i);
          });
          this.itemRows.patchValue(items);
        console.log("aaaa",this.itemRows.value)
      }
        })
  
      }



  initItemRow():FormGroup{
    return this._fb.group({
    projectId:[],
    projectName:[''],
    projectDescription: [''],
    clientId:[ ],
    clientName: [''],
    startDate: [''],
    endDate: [''],
    currentStatus: [''],
    sredProject: [''],
    enabledFlag: ['true'],
    assignedTo:[''],
    activities: [
      {
        activityId: [],}]
    })
  }
  formatDate(input){
    return moment(input).format('YYYY-MM-DD')
  }
  get itemRows() : FormArray {

    return this.projectGroup.get("itemRows") as FormArray
  }

  saveField()
  {
    
    console.log(this.projectGroup.value.itemRows)
    console.log("hi");
    var body = []; //this.projectGroup.value.itemRows;
    // let formObj = this.projectGroup.value; // {name: '', description: ''}
    //     let serializedForm = JSON.stringify(formObj.itemRows);
    //     console.log(serializedForm);
    this.projectGroup.value.itemRows.forEach(element => {
      
      if(element.projectId == null){
        var eid =[];
        if(element.assignedTo.length > 0){

          element.assignedTo.forEach(e => {
            eid.push(e.employeeId);
          });
        }
        console.log("zakkkkkk")
        var item = {
          projectName:element.projectName,
          projectDescription:element.projectDescription,
          clientName:element.clientName,
          clientId:element.clientId,
          startDate:element.startDate,
          endDate:element.endDate,
          currentStatus:element.currentStatus,
          sredProject:element.sredProject,
          enabledFlag:element.enabledFlag,
          assignedTo:element.assignedTo,
          activities:[],
          employeeID:eid,
        }
        body.push(item);
      }
    });
    const url = `${this._url.project.addProject}`

  //   body.forEach(function (value,index) {
  //     body[index].activities = [{activityId: this.selectedactivity}]
  // });
    // body.activities.push({activityId: this.selectedactivity})
    if(body.length > 0){

      console.log("boddddy",body);
      this._http.post(url,body).subscribe(
        {
          next:(res:any)=>{
            this.ProjectItems = res.data;
            console.log(res.responseMessage);
            // this.getListofProjects();
          }
        });
    }
    else{
      this._toast.error('No new project to save','Error saving');
    }
  }


Cancel(itemrow:any){

  if(itemrow.editable == true)
  return itemrow.editable = false
  
}



makeEditable(itemrow: any,) {
  console.log("aww",itemrow);

  if(itemrow.editable == true && itemrow.value.projectId != ''){

  const url = `${this._url.project.editproject}`

  var body = itemrow.value;

  // body.forEach(function (value,index) {
  //   body[index].activities = [{activityId: 4}]
  // });

  console.log(body);
  this._http.post(url,body).subscribe(
    {
      next:(res:any)=>{
        console.log(res.responseMessage);
      }
    });}
    itemrow.editable = !itemrow.editable;
    
  }

public addFieldValue(){
  this.itemRows.push(this.initItemRow());
    console.log("hey");
}

public deleteRow(index : any) {
  
  
  console.log("hi");
  // let formObj = this.projectGroup.value; // {name: '', description: ''}
  //     let serializedForm = JSON.stringify(formObj.itemRows);
  //     console.log(serializedForm);
  const body = this.projectGroup.value.itemRows[index];
  console.log("we",body);
  const url = `${this._url.project.deleteProject}?projectID=${body.projectId}`
  this._http.post(url,body.projectId).subscribe(
    {
      next:(res:any)=>{
        this.itemRows.removeAt(index)
        console.log(res.responseMessage);
      }
    });

   
    this.itemRows.removeAt(index)
//   console.log("hiii");
//   const control = <FormArray>this.projectGroup.controls['itemRows'];
//   if(control != null)
//   {
//     this.TotalRow = control.value.length;
//   }
//   if(this.TotalRow > 1)
//   {
//     control.removeAt(index);
//   }
//   else{
//     alert('one record is mendatory');
//     return false;
// }
 }

 pageChanged(event){
  this.config.currentPage = event;
  this.getListofProjects();
}

public getEmployeeList() {
  const url = `${this._url.Employee.getAllEmployeeList}`;
  this._http.get(url).subscribe({
    next: (res: any) => {
      this.employeeList = res.data;
    },
    error: (msg) => {},
  });
}

public onProjectEmployeeDeSelect(item: any) {
    this.selectedEmployeeList.splice(
      this.selectedEmployeeList.indexOf(item),
    );
  }

public onProjectEmployeeSelect(item: any) {
    this.selectedEmployeeList.push(item);
}


searchItems(event: any) {
  this.config.search = event.target.value;
  this.searchTerm = event.target.value;
  // this.getListofProjects();
}

}
