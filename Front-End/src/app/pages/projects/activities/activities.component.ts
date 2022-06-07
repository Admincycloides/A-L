import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators} from '@angular/forms';
import { Title } from '@angular/platform-browser';


@Component({
  selector: 'app-activities',
  templateUrl: './activities.component.html',
  styleUrls: ['./activities.component.scss']
  
})

export class ActivitiesComponent implements OnInit {
  projectGroup : FormGroup;
  TotalRow : number; 
  public index: any = '';
  public isSubmitted: boolean = false;

  public isVisible : boolean = false;

  public text: string = 'Add Activity';

  constructor(private _fb:FormBuilder, public titleService: Title) {
  }

  ngOnInit(): void {
    this.titleService.setTitle("Project Name");
    this.projectGroup = this._fb.group({
      itemRows:this._fb.array([this.initItemRow()]),
    });
  }

  initItemRow(){
    return this._fb.group({
      ActivityName:[''],
      ActivityDescription:['']
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
    if(this.text == "save"){
  }}

public addFieldValue() { 
  if(this.text =="save"){
 this.isVisible = false;
  const control = <FormArray>this.projectGroup.controls['itemRows'];
  control.push(this.initItemRow());
   console.log("hiii")
  }}

 public deleteRow(index : any) {
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
 }}
