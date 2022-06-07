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
  addProjects = [];
  public isSubmitted: boolean = false;
  projectGroup : FormGroup;
  TotalRow : number; 
  public toggleButton: boolean = false;


  public text: string = 'Add Project';

  constructor(private _url: UrlService,
    private _fb:FormBuilder,private _http: HttpService
  ) { }

  ngOnInit(): void {
    this.projectGroup = this._fb.group({
      itemRows:this._fb.array([this.initItemRow()]),
    });
    this.toggleButton = true
  }

  initItemRow(){
    return this._fb.group({
      projectName: [''],
      projectDescription: [''],
      clientName: [''],
      startDate: [''],
      endDate: [''],
      currentStatus: [''],
      shedoject:[''],
      editable: true,
      isNew: true,
      assignedto:[''],
    })
  }

  saveField()
  {
    console.log("hi");
    let formObj = this.projectGroup.getRawValue(); // {name: '', description: ''}
        let serializedForm = JSON.stringify(formObj);
        console.log(serializedForm);
    const url = `${"http://103.79.223.61:440/" + this._url.Project.addProject}`

    const body = this.addProjects;
    console.log(url);
    this._http.post(url,body).subscribe
      {
        next:(res:any)=>{
          console.log(res.responseMessage);
      
      }
    
  }
  }
//   public changeText(){
//     if (this.text === 'Add Project') {
//       this.text = 'Save';
//     } else {
//       this.text = 'Add Project';
//     }

// }

makeEditable(itemrow: any) {
  itemrow.editable = !itemrow.editable;
  }

public addFieldValue(){
    const control = <FormArray>this.projectGroup.controls['itemRows'];
    control.push(this.initItemRow());
    console.log("hey");
}

}
