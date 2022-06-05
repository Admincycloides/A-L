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
  selectedTimesheet ={}
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
    this.userDetails = JSON.parse(localStorage.getItem('token'));
    this.getEmployeeDetails();
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
      monday:[''],
      tuesday:[''],
      wednesday:[''],
      thursday:[''],
      friday:[''],
      saturday:[''],
      sunday:[''],
      //timeDetails: this._fb.array([]),
      remarks:['']
    });
  }
  private getEmployeeDetails(){
    const url = `${this._url.Employee.getEmployeeDetails}?UserID=${this.userDetails.userId}`
    this._http.get(url).subscribe(
      (res)=>{
        console.log(res);
        //localStorage.setItem('user',JSON.stringify(res.data))
      }
      // {
      //   next(res) {
      //     localStorage.setItem('user',JSON.stringify(res.data));
      //   }
      // }
    )
  }

  public pageChanged(event) {
    this.config.currentPage = event;
    //this.();
  }
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

  //Timesheet Editing and Add
  onTimesheetAddEdit(id:any){
    if(id == -1){
      this.open(this.content);
      this.addTimesheetForm.reset();
      this.selectedTimesheet = {};
    }
    else{
      this.open(this.content);
      this.addTimesheetForm.controls.remarks.setValue(this.timeSheetDetails[id].remarks);
      this.addTimesheetForm.controls.project.setValue(this.timeSheetDetails[id].projectName);
      this.addTimesheetForm.controls.activity.setValue(this.timeSheetDetails[id].activityName);
      this.addTimesheetForm.controls.monday.setValue(this.timeSheetDetails[id].timeTaken[0].numberOfHours);
      this.addTimesheetForm.controls.tuesday.setValue(this.timeSheetDetails[id].timeTaken[1].numberOfHours);
      this.addTimesheetForm.controls.wednesday.setValue(this.timeSheetDetails[id].timeTaken[2].numberOfHours);
      this.addTimesheetForm.controls.thursday.setValue(this.timeSheetDetails[id].timeTaken[3].numberOfHours);
      this.addTimesheetForm.controls.friday.setValue(this.timeSheetDetails[id].timeTaken[4].numberOfHours);
      this.addTimesheetForm.controls.saturday.setValue(this.timeSheetDetails[id].timeTaken[5].numberOfHours);
      this.addTimesheetForm.controls.sunday.setValue(this.timeSheetDetails[id].timeTaken[6].numberOfHours);
      console.log(this.timeSheetDetails[id]);

      this.selectedTimesheet = this.timeSheetDetails[id];

      

      
      //const timeDetails = this.addTimesheetForm.get('timeDetails')['controls'] as FormArray;
      
      // timeDetails.push(this._fb.group({
      //     monday: '',
      //     tuesday: '',
      //     wednesday: 3,
      //     thursday: '',
      //     friday: '',
      //     saturday: '',
      //     sunday: ''
      //   }))
      
      // console.log(this.addTimesheetForm.get('timeDetails')['controls'])
      //this.addTimesheetForm.controls.timeDetails.setValue(timeDetails);
      // this.addTimesheetForm.controls.timeDetails.value.splice(0,0,this.timeSheetDetails[id].timeTaken);
      // this.selectedDates = this.addTimesheetForm.controls.timeDetails.value[0];
      // console.log("hi",this.addTimesheetForm.controls.timeDetails.value[0][0].numberOfHours);
      // this.addTimesheetForm.controls['project'].value = this.timeSheetDetails[id];
      // this.selectedActivityName = this.timeSheetDetails[id];

      // console.log(this.selectedActivityName,this.selectedProjectName);
     
    }


  }
  onTimesheetDelete(id:any){
    // const data = this.timeSheetDetailsArray[id];
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

  }
  onSaveTimesheetDetails(){
    // console.log("project",this.addTimesheetForm.controls.project.value);
    // console.log("activity",this.addTimesheetForm.controls.project.value);
    // console.log("remarks",this.addTimesheetForm.controls.project.value);
    // console.log("remarks",this.addTimesheetForm.controls.timeDetails.value);
    if(!Object.keys(this.selectedTimesheet).length){
      console.log("Add")
    }else{
      let timeTaken = this.selectedTimesheet['timeTaken'];
      timeTaken[0].numberOfHours = this.addTimesheetForm.controls.monday.value;
      timeTaken[1].numberOfHours = this.addTimesheetForm.controls.tuesday.value;
      timeTaken[2].numberOfHours = this.addTimesheetForm.controls.wednesday.value;
      timeTaken[3].numberOfHours = this.addTimesheetForm.controls.thursday.value;
      timeTaken[4].numberOfHours = this.addTimesheetForm.controls.friday.value;
      timeTaken[5].numberOfHours = this.addTimesheetForm.controls.saturday.value;
      timeTaken[6].numberOfHours =this.addTimesheetForm.controls.sunday.value;

      const data = {
      //status          : this.selectedTimesheet.status,
      activityName    : this.addTimesheetForm.controls.activity.value,
      projectName     : this.addTimesheetForm.controls.project.value,
      timeTaken       : timeTaken,
      remarks         : this.addTimesheetForm.controls.remarks.value
      }
      console.log("data",data);


    }

    
  }
  selectChanges(value:any){

  }

  
}

