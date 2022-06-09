import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators} from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { HttpService } from 'app/_services/http.service';
import { UrlService } from 'app/_services/url.service';


@Component({
  selector: 'app-activities',
  templateUrl: './activities.component.html',
  styleUrls: ['./activities.component.scss']
  
})

export class ActivitiesComponent implements OnInit {
  projectGroup : FormGroup;
  TotalRow : number; 
  userDetails:any;
  public index: any = '';
  public isSubmitted: boolean = false;

  public addActivity = [];

  public isVisible : boolean = false;

  public text: string = 'Add Activity';

  constructor(private _fb:FormBuilder, public titleService: Title,private _http: HttpService,private _url: UrlService) {
    this.projectGroup = this._fb.group({
      itemRows:this._fb.array([this.initItemRow()]),
    });
  }

  ngOnInit() {
    this.titleService.setTitle("Project Name");
    this.getActivityDetails()
  }
  private getActivityDetails(){


    const url = `${this._url.activity.getActivityList}`
    this._http.get(url).subscribe({
      next:(res:any)=>{
        console.log("res",res)
        this.projectGroup['controls'].itemRows['controls'] = res.data;
      }

      })
    }

  initItemRow():FormGroup{
    return this._fb.group({
      activityName:[""],
      activityDescription:[""],
      enabledFlag: [""]
    })
  }

  get itemRows() : FormArray {

    return this.projectGroup.get("itemRows") as FormArray
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

    // console.log(this.projectGroup.value)
    // console.log(this.projectGroup['controls'].itemRows['controls']);

  //  let temp = [
  //   {
  //     "activityId": 2989,
  //     "activityName": "Activity1",
  //     "activityDescription": "First Activity "
  //   }
  // ]
      if(this.text == "Add Activity"){
      const body = this.projectGroup.value.itemRows;
        console.log(body);
      let formObj = this.projectGroup.value;
      console.log(formObj);
        let serializedForm = JSON.stringify(formObj.itemRows);
        console.log(serializedForm);
      const url = `${this._url.activity.addActivity}`

    // const body = this.addActivity;
    // console.log(body);
    this._http.post(url,body).subscribe(
      {
        next:(res:any)=>{
          console.log(res.responseMessage);
        }
      });
    }
  }

public addFieldValue() { 
  if(this.text == "save"){
    this.isVisible = true;
  this.itemRows.push(this.initItemRow());
  }
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
}
