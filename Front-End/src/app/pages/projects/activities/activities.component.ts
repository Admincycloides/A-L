import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators} from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Router,ActivatedRoute }  from '@angular/router';
import { HttpService } from 'app/_services/http.service';
import { UrlService } from 'app/_services/url.service';
// import { ProjectsComponent } from '../projects.component';


@Component({
  selector: 'app-activities',
  templateUrl: './activities.component.html',
  styleUrls: ['./activities.component.scss']
  
})

export class ActivitiesComponent implements OnInit {
  projectGroup : FormGroup;
  TotalRow : number; 
  activityItems:any;
  projectId:any;
  sub:any;
  getselectedactivity:any;
  userDetails:any;
  public index: any = '';
  public isSubmitted: boolean = false;

  // public addActivity = [];

  public isVisible : boolean = false;

  public text: string = 'Add Activity';

  config: any;
  collection = { count: 60, data: [] }
  
  

  constructor(private _fb:FormBuilder, public titleService: Title,private _http: HttpService,private _url: UrlService,private _router:Router,private _Activatedroute:ActivatedRoute,
    ) {
    this.projectGroup = this._fb.group({
      itemRows:this._fb.array([]),
    });

    this.getActivityDetails(),

    this.config = {
      itemsPerPage: 5,
      currentPage: 1,
      totalItems: this.collection.count
    };
  }

  get itemRows() : FormArray {

    return this.projectGroup.get("itemRows") as FormArray
  }

  initItemRow():FormGroup{
    return this._fb.group({
      // activityId:[],
      activityName:[""],
      activityDescription:[""],
      enabledFlag: []
    })
  }



  ngOnInit() {
    this.titleService.setTitle("Project Name");
    
    this.sub=this._Activatedroute.paramMap.subscribe(params => { 
      console.log(params);
       this.projectId = params.get('ProjectId'); 
       console.log("id",this.projectId)
       this.getActivityforProject(this.projectId);
    // this.getActivityDetails();
           
   });
  }
  
  public getActivityforProject(projectId){
    console.log("qqqq")
    const url = `${this._url.project.getprojectdetailsByid}?ProjectID=${projectId}`
    this._http.get(url).subscribe({
      next:(res:any)=>{
        // this.getselectedactivity = res.data.activities;
        var items = [];
        let activities = res.data.activities
        res.data.activities.forEach(element => {
          this.addFieldValue()
          let i = {
                enabledFlag:element.enabledFlag,
                // activityId:element.activityId,
                activityName:element.activityName,
                activityDescription:element.activityDescription
          }
          items.push(i);
        });
        console.log("aaaa",res.data.activities)
        this.itemRows.setValue(items);
        console.log("aaaa",this.itemRows.value)
      }
    })
  }

  private getActivityDetails(){
    const url = `${this._url.activity.getActivityList}`
    this._http.get(url).subscribe({
      next:(res:any)=>{
        var items = [];
          // res.data.forEach(element => {
          //   this.addFieldValue();
          //     let i = {
          //       activityId:element.activityId,
          //       activityName:element.activityName,
          //       activityDescription:element.activityDescription
          //     }
          //     items.push(i);
          // });
          // this.itemRows.patchValue(items);
        // console.log("aaaa",this.itemRows.value)
      }
        })
    }
  
  public changeText(){
    if (this.text === 'Add Activity') {
      this.text = 'save';
    } else {
      this.text = 'Add Activity';
    }

}

// public disable() {
//   if (this.text == "save"){
//     this.toggleButton = true
//   }
//   else if (this.text ==  'Add Activity'){
//     this.toggleButton = true
//   }
// }

makeEditable(itemrow: any) {
  itemrow.editable = !itemrow.editable;
  }

  saveField(){
      if(this.text == "Add Activity"){
      const body = this.projectGroup.value.itemRows;
        console.log(body);
      // let formObj = this.projectGroup.value.itemRows;
      // console.log(formObj);
      //   let serializedForm = JSON.stringify(formObj.itemRows);
      //   console.log(serializedForm);
      const url = `${this._url.activity.addActivity}`

    // const body = this.addActivity;
    // console.log(body);
    this._http.post(url,body).subscribe(
      {
        next:(res:any)=>{
          this.activityItems = res.data;
          console.log(res.responseMessage);
        }
      });
    }
  }

public addFieldValue() { 
  if(this.text == "save"){
    this.isVisible = true;
  }
  this.itemRows.push(this.initItemRow());
  // const control = <FormArray>this.projectGroup.controls['itemRows'];
  // control.push(this.initItemRow());
  //  console.log("hiii")}
}

 public deleteRow(index : any) {
  console.log("hiii");
  this.itemRows.removeAt(index)
//   const control = <FormArray>this.projectGroup.controls['itemRows'];
//   // control.removeAt(index);
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

