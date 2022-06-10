import { Component, OnInit,NgModule } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HttpService } from 'app/_services/http.service';
import { UrlService } from 'app/_services/url.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.scss']
})
export class ProjectsComponent implements OnInit {
  public index: any = '';
  userDetails: any;
  
  public isSubmitted: boolean = false;
  projectGroup : FormGroup;
  TotalRow : number; 
  public toggleButton: boolean = false;

  // public addProjects = [];

  public text: string = 'Add Project';
  config: any;
  collection = { count: 60, data: [] };

  constructor(private _url: UrlService,
    private _fb:FormBuilder,private _http: HttpService
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
    this.toggleButton = true,
    this.getProjectActivityDetails(),
    this.getListofProjects()
  }

  private getProjectActivityDetails(){

    const url = `${this._url.project.getprojectListbyEmployeeID}`
    this._http.get(url).subscribe({
      next:(res:any)=>{
        this.projectGroup.value.itemRows = res.data;}
      })
    }


    private getListofProjects(){

      const url = `${this._url.project.getallprojectlist}`
    this._http.get(url).subscribe({
      next:(res:any)=>{
        this.projectGroup.value.itemRows = res.data;}
      })

    }



  initItemRow():FormGroup{
    return this._fb.group({
    projectName:[""],
    projectDescription: [""],
    clientId: [0],
    startDate: ["29/11/2022"],
    endDate: ["29/12/2022"],
    currentStatus: [""],
    sredProject: [""],
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

    // let temp = [
    //   {
    //     "projectName": "prr",
    //     "projectDescription": "prr",
    //     "clientId": 1,
    //     "startDate": "2022-06-08T14:12:39.719Z",
    //     "endDate": "2022-06-08T14:12:39.719Z",
    //     "currentStatus": "prr",
    //     "sredProject": "prr",
    //     "activities": [
    //       {

    //         "activityName": "prr",
    //         "activityDescription": "prr",
    //       }
    //     ]
    //   }
    // ]
    console.log("hi");
    let formObj = this.projectGroup.value; // {name: '', description: ''}
        let serializedForm = JSON.stringify(formObj.itemRows);
        console.log(serializedForm);
    const url = `${this._url.project.addProject}`

    const body = this.projectGroup.value.itemRows;
    console.log(body);
    this._http.post(url,body).subscribe(
      {
        next:(res:any)=>{
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
  
  // let temp = [
  //   {
  //     "projectName": "prr",
  //     "projectDescription": "prr",
  //     "clientId": 1,
  //     "startDate": "2022-06-08T14:12:39.719Z",
  //     "endDate": "2022-06-08T14:12:39.719Z",
  //     "currentStatus": "prr",
  //     "sredProject": "prr",
  //     "activities": [
  //       {

  //         "activityName": "prr",
  //         "activityDescription": "prr",
  //       }
  //     ]
  //   }
  // ]
  console.log("hi");
  let formObj = this.projectGroup.value; // {name: '', description: ''}
      let serializedForm = JSON.stringify(formObj.itemRows);
      console.log(serializedForm);
  const url = `${this._url.project.deleteProject}`

  const body = this.projectGroup.value.itemRows;
  console.log(body);
  this._http.post(url,body).subscribe(
    {
      next:(res:any)=>{
        console.log(res.responseMessage);
      }
    });
  console.log("hiii");
  const control = <FormArray>this.projectGroup.controls['itemRows'];
  if(control != null)
  {
    this.TotalRow = control.value.length;
  }
  if(this.TotalRow > 1)
  {
    control.removeAt(index);
  }
  else{
    alert('one record is mendatory');
    return false;
}
 }

 pageChanged(event){
  this.config.currentPage = event;
}

}
