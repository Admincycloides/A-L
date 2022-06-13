import { Component, OnInit,NgModule } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HttpService } from 'app/_services/http.service';
import { UrlService } from 'app/_services/url.service';
import { data } from 'jquery';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.scss']
})
export class ProjectsComponent implements OnInit {
  public index: any = '';
  userDetails: any;
  getActivityName:any;
  ProjectItems:any;
  selectedactivity:any;
  
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

  public text: string = 'Add Project';
  config: any;
  collection = { count: 60, data: [] };

  constructor(private _url: UrlService,
    private _fb:FormBuilder,private _http: HttpService
  ) { 
    this.projectGroup = this._fb.group({
      itemRows:this._fb.array([this.initItemRow()]),
    });

    this.config = {
      itemsPerPage: 5,
      currentPage: 1,
      totalItems: this.collection.count
    };
  }
  

  ngOnInit(): void {
    this.toggleButton = true,
    this.getProjectActivityDetails(),
    this.getListofProjects(),
    this.getActivityLists()
  }

  private getProjectActivityDetails(){

    const url = `${this._url.project.getprojectListbyEmployeeID}`
    this._http.get(url).subscribe({
      next:(res:any)=>{
        this.itemRows.controls = (res.data);}
      })
    }

    private getActivityLists(){
      console.log("first call")
      const url = `${this._url.activity.getActivityList}`
      this._http.get(url).subscribe({next:(res:any)=>{
          this.getActivityName = res.data;
          console.log("data",res.data)}
        })
        }



    private getListofProjects(){

      const url = `${this._url.project.getallprojectlist}`
    this._http.get(url).subscribe({
      next:(res:any)=>{
        this.itemRows.controls = (res.data);
      console.log("aaaa",res.data)
    }
      })

    }



  initItemRow():FormGroup{
    return this._fb.group({
    projectName:new FormControl('',Validators.required),
    projectDescription: new FormControl('',Validators.required),
    clientId: [0],
    startDate: new FormControl('29/11/2022',Validators.required),
    endDate: new FormControl('29/12/2022',Validators.required),
    currentStatus: new FormControl('',Validators.required),
    sredProject: new FormControl('',Validators.required),
    activities: [
     ] 
    })
  }

  get itemRows() : FormArray {

    return this.projectGroup.get("itemRows") as FormArray
  }

  saveField()
  {
    console.log(this.projectGroup.value.itemRows)
    console.log("hi");
    // let formObj = this.projectGroup.value; // {name: '', description: ''}
    //     let serializedForm = JSON.stringify(formObj.itemRows);
    //     console.log(serializedForm);
    const url = `${this._url.project.addProject}`

    var body = this.projectGroup.value.itemRows;

    body.forEach(function (value,index) {
      body[index].activities = [{activityId: this.selectedactivity}]
  });
    // body.activities.push({activityId: this.selectedactivity})
    console.log("boddddy",body);
    this._http.post(url,body).subscribe(
      {
        next:(res:any)=>{
          this.ProjectItems = res.data;
          console.log(res.responseMessage);
        }
      });
  }
  public changeText(){
    if (this.text === 'Add Project') {
      this.text = 'save';
    } else {
      this.text = 'Add Project';
    }

}

makeEditable(itemrow: any) {
  itemrow.editable = !itemrow.editable;
  }

public addFieldValue(){
  this.itemRows.push(this.initItemRow());
    // const control = <FormArray>this.projectGroup.controls['itemRows'];
    // control.push(this.initItemRow());
    console.log("hey");
}

public deleteRow(index : any) {
  
  console.log("hi");
  // let formObj = this.projectGroup.value; // {name: '', description: ''}
  //     let serializedForm = JSON.stringify(formObj.itemRows);
  //     console.log(serializedForm);
  const url = `${this._url.project.deleteProject}`

  const body = this.projectGroup.value.itemRows[index];
  console.log(body);
  this._http.post(url,[body]).subscribe(
    {
      next:(res:any)=>{
        this.itemRows.removeAt(index)
        console.log(res.responseMessage);
      }
    });
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
}

}
