import { Component, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HttpService } from 'app/_services/http.service';
import { UrlService } from 'app/_services/url.service';
import * as moment from 'moment';



@Component({
  selector: 'app-timesheet',
  templateUrl: './timesheet.component.html',
  moduleId: module.id,
  styleUrls: ['./timesheet.component.scss']
})
export class TimesheetComponent implements OnInit {
  @ViewChild('content') content: any;
  closeResult = '';
  userDetails: any;
  startOfWeek: any;
  endOfWeek: any;
  
  timeSheetDetails:any[]
  currentWeek :any[];
  public config = {
    id: 'timesheet',
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 1,
  };
  //timeSheetDetailsArray = new Array(11);
  timeSheetDetailsArray =[];
  model;
  weekShow:any;
  selectAllTimesheet: boolean = false;
  addTimesheetForm :FormGroup;
  projectList:any[];
  activityList:any[];
  selectedProjectName:any;
  selectedDates: any[];
  get f() {
    return this.addTimesheetForm.controls;
  }

  constructor(private _url: UrlService,
    private _http: HttpService,private modalService: NgbModal,
    private _fb: FormBuilder,
    ) { }
    

  ngOnInit(): void {
    console.log(this.content);
    this.projectList = ['Project 1','Project 2','Project 3','Project 4'];
    this.activityList = ['Activity 1','Activity 2','Activity 3','Activity 4'];
    // this.userDetails = JSON.parse(localStorage.getItem('token'));
    // this.getEmployeeDetails();
    this.startOfWeek = moment().startOf('isoWeek').toDate();
    this.endOfWeek = moment().endOf('isoWeek').toDate();
    this.weekShow = moment(this.startOfWeek).format("MMMM-DD")+"-"+moment(this.endOfWeek).format("MMMM-DD");
    this.currentWeek = this.dateFormatter(moment(this.startOfWeek).format("YYYY-MM-DD"),moment(this.endOfWeek).format("YYYY-MM-DD"))
    this.timeSheetDetails= [
      {
          "projectId": 9,
          "projectName": "Project 1",
          "activityId": 6,
          "activityName": "Activity 1",
          "timeTaken": [
              {
                  "date": "2022-05-30T00:00:00",
                  "numberOfHours": 2
              },
              {
                  "date": "2022-05-31T00:00:00",
                  "numberOfHours": 1
              },
              {
                  "date": "2022-06-01T00:00:00",
                  "numberOfHours": 3
              },
              {
                  "date": "2022-06-02T00:00:00",
                  "numberOfHours": 4
              },
              {
                  "date": "2022-06-03T00:00:00",
                  "numberOfHours": 0.25
              },
              {
                  "date": "2022-06-04T00:00:00",
                  "numberOfHours": 2
              },
              {
                  "date": "2022-06-05T00:00:00",
                  "numberOfHours": 0
              }
          ]
      },
      {
          "projectId": 10,
          "projectName": "Project 2",
          "activityId": 10,
          "activityName": "Activity 2",
          "timeTaken": [
              {
                  "date": "2022-05-30T00:00:00",
                  "numberOfHours": 2
              },
              {
                  "date": "2022-05-31T00:00:00",
                  "numberOfHours": 1
              },
              {
                  "date": "2022-06-01T00:00:00",
                  "numberOfHours": 3
              },
              {
                  "date": "2022-06-02T00:00:00",
                  "numberOfHours": 4
              },
              {
                  "date": "2022-06-03T00:00:00",
                  "numberOfHours": 0.25
              },
              {
                  "date": "2022-06-04T00:00:00",
                  "numberOfHours": 2
              },
              {
                  "date": "2022-06-05T00:00:00",
                  "numberOfHours": 0
              }
          ]
      },
      {
          "projectId": 10,
          "projectName": "Project 3",
          "activityId": 12,
          "activityName": "Activity 3",
          "timeTaken": [
              {
                  "date": "2022-05-30T00:00:00",
                  "numberOfHours": 4
              },
              {
                  "date": "2022-05-31T00:00:00",
                  "numberOfHours": 2
              },
              {
                  "date": "2022-06-01T00:00:00",
                  "numberOfHours": 2
              },
              {
                  "date": "2022-06-02T00:00:00",
                  "numberOfHours": 2
              },
              {
                  "date": "2022-06-03T00:00:00",
                  "numberOfHours": 4
              },
              {
                  "date": "2022-06-04T00:00:00",
                  "numberOfHours": 2
              },
              {
                  "date": "2022-06-05T00:00:00",
                  "numberOfHours": 0
              }
          ]
      },
      {
          "projectId": 10,
          "projectName": "Project 3",
          "activityId": 13,
          "activityName": "Activity 2",
          "timeTaken": [
              {
                  "date": "2022-05-30T00:00:00",
                  "numberOfHours": 0
              },
              {
                  "date": "2022-05-31T00:00:00",
                  "numberOfHours": 0
              },
              {
                  "date": "2022-06-01T00:00:00",
                  "numberOfHours": 0
              },
              {
                  "date": "2022-06-02T00:00:00",
                  "numberOfHours": 0
              },
              {
                  "date": "2022-06-03T00:00:00",
                  "numberOfHours": 1
              },
              {
                  "date": "2022-06-04T00:00:00",
                  "numberOfHours": 0
              },
              {
                  "date": "2022-06-05T00:00:00",
                  "numberOfHours": 0
              }
          ]
      },
      {
          "projectId": 10,
          "projectName": "Project 3",
          "activityId": 14,
          "activityName": "Activity 4",
          "timeTaken": [
              {
                  "date": "2022-05-30T00:00:00",
                  "numberOfHours": 0
              },
              {
                  "date": "2022-05-31T00:00:00",
                  "numberOfHours": 0
              },
              {
                  "date": "2022-06-01T00:00:00",
                  "numberOfHours": 0
              },
              {
                  "date": "2022-06-02T00:00:00",
                  "numberOfHours": 0
              },
              {
                  "date": "2022-06-03T00:00:00",
                  "numberOfHours": 0
              },
              {
                  "date": "2022-06-04T00:00:00",
                  "numberOfHours": 2
              },
              {
                  "date": "2022-06-05T00:00:00",
                  "numberOfHours": 0
              }
          ]
      },
      {
          "projectId": 10,
          "projectName": "Project 4",
          "activityId": 16,
          "activityName": "Activity 3",
          "timeTaken": [
              {
                  "date": "2022-05-30T00:00:00",
                  "numberOfHours": 0
              },
              {
                  "date": "2022-05-31T00:00:00",
                  "numberOfHours": 0
              },
              {
                  "date": "2022-06-01T00:00:00",
                  "numberOfHours": 0
              },
              {
                  "date": "2022-06-02T00:00:00",
                  "numberOfHours": 0
              },
              {
                  "date": "2022-06-03T00:00:00",
                  "numberOfHours": 0.1
              },
              {
                  "date": "2022-06-04T00:00:00",
                  "numberOfHours": 0
              },
              {
                  "date": "2022-06-05T00:00:00",
                  "numberOfHours": 0
              }
          ]
      }
  ]
    this.getTimesheetDetails();
    this.addTimesheetForm =this._fb.group({
      project: ['', Validators.required],
      activity: ['', Validators.required],
      timeDetails: this._fb.array([
        new FormControl(),
        new FormControl(),
        new FormControl(),
      ]),
      remarks:['']
    });
  }
  private getEmployeeDetails(){

    this._http.get(`${this._url.Employee.getEmployeeDetails}?UserID=${this.userDetails.userId}`).subscribe(
      {
        next(res) {
          //localStorage.setItem('user',JSON.stringify(res.data));
        }
      }
    )
  }

  public pageChanged(event) {
    this.config.currentPage = event;
    //this.();
  }
  // [
  //   [projectName,projectID,monday,tues,]
  //   [projectName,projectID,monday,tues,]
  //   [projectName,projectID,monday,tues,]
  // ]
  getTimesheetDetails(){


    // this.timeSheetDetails.forEach(
    //   (item,index)=>{
    //     let rowArray =[];
    //     let itemIndex = this.currentWeek.indexOf(moment(item.Date).format("MMMM-DD"));
    //     rowArray.push(item.Status,item.ProjectName,item.ActivityName);
    //     while(rowArray.length < 10){
    //       if(rowArray.length != 10){
    //         if(rowArray.length == itemIndex+3){
    //           rowArray.push(item.NumberOfHours);
    //         }
    //         else{
    //           rowArray.push(0);
    //         }
    //       }
    //     }
    //     // rowArray.splice(itemIndex+2,0,item.NumberOfHours);
    //     // rowArray.splice(9,0,5);
    //     rowArray.push(item.NumberOfHours,item.Remarks,item.UniqueId);
    //      this.timeSheetDetailsArray.push(rowArray);
    //   }
      
    // )
  }
  dateChange(event){
    const date = event.month +'-'+event.day+"-"+event.year
    this.startOfWeek = moment(date).startOf('isoWeek').toDate();
    this.endOfWeek = moment(date).endOf('isoWeek').toDate();
    this.model = "2022-12-5";
    this.currentWeek = this.dateFormatter(moment(this.startOfWeek).format("YYYY-MM-DD"),moment(this.endOfWeek).format("YYYY-MM-DD"));
    this.weekShow = moment(this.startOfWeek).format("MMMM-DD")+"-"+moment(this.endOfWeek).format("MMMM-DD");
    //console.log("this.currentWeek ",this.currentWeek )
  }
  private dateFormatter(start:any,end:any){
    var dateArray = [];
    var currentDate = moment(start);
    var stopDate = moment(end);
    while(currentDate<=stopDate){
      dateArray.push(moment(currentDate).format("MMMM-DD"));
      currentDate = moment(currentDate).add(1,'days');
    }
    return dateArray;
  }
  onPreviousClick(){
    this.startOfWeek = moment(this.startOfWeek).subtract(1,'weeks');
    this.endOfWeek = moment(this.endOfWeek).subtract(1,'weeks');
    this.currentWeek = this.dateFormatter(moment(this.startOfWeek).format("YYYY-MM-DD"),moment(this.endOfWeek).format("YYYY-MM-DD"));
    this.weekShow = moment(this.startOfWeek).format("MMMM-DD")+"-"+moment(this.endOfWeek).format("MMMM-DD")
    //console.log("this.currentWeek ",this.currentWeek )
  }
  onNextClick(){
    this.startOfWeek = moment(this.startOfWeek).add(1,'weeks');
    this.endOfWeek = moment(this.endOfWeek).add(1,'weeks');
    this.currentWeek = this.dateFormatter(moment(this.startOfWeek).format("YYYY-MM-DD"),moment(this.endOfWeek).format("YYYY-MM-DD"));
    this.weekShow = moment(this.startOfWeek).format("MMMM-DD")+"-"+moment(this.endOfWeek).format("MMMM-DD");
    //console.log("this.currentWeek ",this.currentWeek )

  }
  

  public open(content: any) {
    console.log("open",content);
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', size: 'xl' }).result.then(
      (result) => {
        this.closeResult = `Closed with: ${result}`;
      },
      (reason) => {
        this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
      }
    );
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  onTimesheetAddEdit(id:any){
    if(id == -1){
      this.open(this.content);
      // this.selectedProjectName ='';
      // this.selectedActivityName = '';
    }
    else{
      this.open(this.content);
      console.log(this.timeSheetDetails[id].projectName);
      this.addTimesheetForm.controls.remarks.setValue('abccc');
      this.addTimesheetForm.controls.project.setValue(this.timeSheetDetails[id].projectName);
      this.addTimesheetForm.controls.activity.setValue(this.timeSheetDetails[id].activityName);
      // const timeDetails = this.addTimesheetForm.get('timeDetails')['controls'] as FormArray;
      
      // timeDetails[0]= this._fb.group({
      //     monday: 6,
      //     tuesday: '',
      //     wednesday: '',
      //     thursday: '',
      //     friday: '',
      //     saturday: '',
      //     sunday: ''
      //   })
      
      // console.log(this.addTimesheetForm.get('timeDetails')['controls'])
      // this.addTimesheetForm.controls.timeDetails.setValue(timeDetails);
      this.addTimesheetForm.controls.timeDetails.value.splice(0,0,this.timeSheetDetails[id].timeTaken);
      this.selectedDates = this.addTimesheetForm.controls.timeDetails.value[0];
      console.log("hi",this.addTimesheetForm.controls.timeDetails.value[0][0].numberOfHours);
      // this.addTimesheetForm.controls['project'].value = this.timeSheetDetails[id];
      // this.selectedActivityName = this.timeSheetDetails[id];

      // console.log(this.selectedActivityName,this.selectedProjectName);
    }


  }
  onTimesheetDelete(id:any){
    // const uniqueId = this.timeSheetDetailsArray[id].UniqueId;
    // this._http.delete(`${this._url.timesheet.deleteTimesheet}/${uniqueId}`).subscribe(
    // {
    //   next(res) {
    //     //this.toastr.success(res.responseMessage)
    // }
    // });
  }

  onRowCheck(id: any){
    console.log(id)
    if(id == -1){
      this.selectAllTimesheet = !this.selectAllTimesheet;
    }else{
      console.log(this.timeSheetDetailsArray[id])
    }
    //console.log(this.selectAllTimesheet)

  }
  onSaveTimesheetDetails(){
    console.log(this.addTimesheetForm.controls['timeDetails'].value);
  }
  selectChanges(value:any){

  }

  
}

