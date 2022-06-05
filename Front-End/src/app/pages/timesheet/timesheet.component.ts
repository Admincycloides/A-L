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
  selectedTimesheet ={};
  superVisorList = [];
  selectedTimesheetRow = [];
  managerId: any;
  timesheetRemarks :any;
  submitRemarks :any;

  get f() {
    return this.addTimesheetForm.controls;
  }

  constructor(private _url: UrlService,
    private _http: HttpService,private modalService: NgbModal,
    private _fb: FormBuilder,
    ) { }
    

  ngOnInit(): void {
    //this.projectList = ['Project 1','Project 2','Project 3','Project 4'];
    //this.activityList = ['Activity 1','Activity 2','Activity 3','Activity 4'];
    this.userDetails = JSON.parse(localStorage.getItem('token'));
    this.getEmployeeDetails();
    this.getSupervisorDetails();
    this.getProjectActivityDetails();
    this.startOfWeek = moment().startOf('isoWeek').toDate();
    this.endOfWeek = moment().endOf('isoWeek').toDate();
    this.weekShow = moment(this.startOfWeek).format("MMMM-DD")+"-"+moment(this.endOfWeek).format("MMMM-DD");
    this.currentWeek = this.dateFormatter(moment(this.startOfWeek).format("YYYY-MM-DD"),moment(this.endOfWeek).format("YYYY-MM-DD"));




    this.timeSheetDetails= [
      {
        "projectId": 9,
        "projectName": 'Glyphosate',
        "activityId": 6,
        "activityName": "Activity 2",
        "status": "Submitted",
        "remarks": "None",
        "timeTaken": [
            {
                "date": "2022-05-30T00:00:00",
                "numberOfHours": 0,
                "uniqueId": 88
            },
            {
                "date": "2022-05-31T00:00:00",
                "numberOfHours": 0,
                "uniqueId": 89
            },
            {
                "date": "2022-06-01T00:00:00",
                "numberOfHours": 0,
                "uniqueId": 90
            },
            {
                "date": "2022-06-02T00:00:00",
                "numberOfHours": 0,
                "uniqueId": 91
            },
            {
                "date": "2022-06-03T00:00:00",
                "numberOfHours": 0,
                "uniqueId": 92
            },
            {
                "date": "2022-06-04T00:00:00",
                "numberOfHours": 0,
                "uniqueId": 93
            },
            {
                "date": "2022-06-05T00:00:00",
                "numberOfHours": 0,
                "uniqueId": 94
            }
        ]
    },
    {
        "projectId": 9,
        "projectName": 'Glyphosate Phase 2',
        "activityId": 15,
        "activityName": "Activity 2",
        "status": "In progress",
        "remarks": "None",
        "timeTaken": [
            {
                "date": "2022-05-30T00:00:00",
                "numberOfHours": 2,
                "uniqueId": 113
            },
            {
                "date": "2022-05-31T00:00:00",
                "numberOfHours": 1,
                "uniqueId": 114
            },
            {
                "date": "2022-06-01T00:00:00",
                "numberOfHours": 3,
                "uniqueId": 115
            },
            {
                "date": "2022-06-02T00:00:00",
                "numberOfHours": 4,
                "uniqueId": 116
            },
            {
                "date": "2022-06-03T00:00:00",
                "numberOfHours": 0.25,
                "uniqueId": 117
            },
            {
                "date": "2022-06-04T00:00:00",
                "numberOfHours": 2,
                "uniqueId": 118
            },
            {
                "date": "2022-06-05T00:00:00",
                "numberOfHours": 0,
                "uniqueId": 119
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
  private getSupervisorDetails(){

    this.superVisorList = [
      {
          "employeeId": "92S5000000009",
          "firstName": "wefwe",
          "lastName": "wefwefw",
          "emailAddress": "p@gmail.com",
          "contactNumber": "1234567890",
          "supervisorFlag": "Y",
          "managerId": "92S5000000009",
          "enabledFlag": "Enabled"
      },
      {
          "employeeId": "92S5000000157",
          "firstName": "wefwe",
          "lastName": "wefwefwe",
          "emailAddress": "dawea@gmail.com",
          "contactNumber": "1234567890",
          "supervisorFlag": "Y",
          "managerId": "92S5000000157",
          "enabledFlag": "Enabled"
      },
      {
          "employeeId": "92S5000000165",
          "firstName": "wefevv",
          "lastName": "wefwefw",
          "emailAddress": "dss@gmail.com",
          "contactNumber": "1234567890",
          "supervisorFlag": "Y",
          "managerId": "92S5000000165",
          "enabledFlag": "Enabled"
      }
  ]


  }

  private getProjectActivityDetails(){
    this.projectList =[{
      "ProjectId": 0,
      "ProjectName": 'Glyphosate',
      "ProjectDescription": null,
      "ClientId": 0,
      "StartDate": "0001-01-01T00:00:00",
      "EndDate": null,
      "CurrentStatus": null,
      "SredProject": null,
      "Activities": [
          {
              "ActivityId": 0,
              "ActivityName": 'Activity 2',
              "ActivityDescription": null
          },
          {
              "ActivityId": 0,
              "ActivityName": 'Activity 2',
              "ActivityDescription": null
          }
      ]
  },{
      "ProjectId": 0,
      "ProjectName": 'Glyphosate Phase 2',
      "ProjectDescription": null,
      "ClientId": 0,
      "StartDate": "0001-01-01T00:00:00",
      "EndDate": null,
      "CurrentStatus": null,
      "SredProject": null,
      "Activities": [
          {
              "ActivityId": 0,
              "ActivityName": 'Activity 2',
              "ActivityDescription": null
          },
          {
              "ActivityId": 0,
              "ActivityName": 'Activity 2',
              "ActivityDescription": null
          }
      ]
  },
  {
      "ProjectId": 0,
      "ProjectName": null,
      "ProjectDescription": null,
      "ClientId": 0,
      "StartDate": "0001-01-01T00:00:00",
      "EndDate": null,
      "CurrentStatus": null,
      "SredProject": null,
      "Activities": [
          {
              "ActivityId": 0,
              "ActivityName": 'Activity 2',
              "ActivityDescription": null
          },
          {
              "ActivityId": 0,
              "ActivityName": 'Activity 2',
              "ActivityDescription": null
          }
      ]
  }
  
  ]
  }

  public onProjectSelect(event:any){
    const project = event.target.value;
    console.log(typeof(project));
    if(project === 'Select Project') this.activityList =[];
    else{
      this.projectList.forEach((item)=>{
        if(item.ProjectName == project) this.activityList = item.Activities;
      })
    }
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
    this.selectedTimesheetRow = [];
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
      //dateArray.push(moment(currentDate).format("MMMM-DD"));
      dateArray.push(currentDate);
      currentDate = moment(currentDate).add(1,'days');
    }
    return dateArray;
  }
  onPreviousClick(){
    this.selectedTimesheetRow = [];
    this.startOfWeek = moment(this.startOfWeek).subtract(1,'weeks');
    this.endOfWeek = moment(this.endOfWeek).subtract(1,'weeks');
    this.currentWeek = this.dateFormatter(moment(this.startOfWeek).format("YYYY-MM-DD"),moment(this.endOfWeek).format("YYYY-MM-DD"));
    this.weekShow = moment(this.startOfWeek).format("MMMM-DD")+"-"+moment(this.endOfWeek).format("MMMM-DD")
    //console.log("this.currentWeek ",this.currentWeek )
  }
  onNextClick(){
    this.selectedTimesheetRow = [];
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
      console.log(id)
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
      console.log(this.addTimesheetForm.controls.activity.value);

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
  // Deleting Timesheet row
  onTimesheetDelete(id:any){
    const data = this.timeSheetDetails[id];
    console.log("delete data",data)
        // const data = this.timeSheetDetails[id];
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
      // this.selectedTimesheetRow.includes(this.timeSheetDetails[id])? this.selectedTimesheetRow.push(this.timeSheetDetails[id])
      // :this.selectedTimesheetRow.
      if(!this.selectedTimesheetRow.includes(this.timeSheetDetails[id]))
      {
        console.log("hi")
        this.selectedTimesheetRow.push(this.timeSheetDetails[id]);
      }else{
        this.selectedTimesheetRow.splice(this.selectedTimesheetRow.indexOf(this.timeSheetDetails[id]),1);
      }
      console.log(this.selectedTimesheetRow);
    }

  }
  //saving the timsheet details
  onSaveTimesheetDetails(){
    this.activityList = [];
    if(!Object.keys(this.selectedTimesheet).length){
      let date = [];
      //console.log(moment(this.currentWeek[0]).format("YYYY-MM-DDT00:00:00"));
      this.currentWeek.forEach((day)=>{date.push(moment(day).format("YYYY-MM-DDT00:00:00"))});
      //console.log("week",date);
      const timeTaken = [
        {date : date[0],numberOfHours: this.addTimesheetForm.controls.monday.value},
        {date : date[1],numberOfHours: this.addTimesheetForm.controls.tuesday.value},
        {date : date[2],numberOfHours: this.addTimesheetForm.controls.wednesday.value},
        {date : date[3],numberOfHours: this.addTimesheetForm.controls.thursday.value},
        {date : date[4],numberOfHours: this.addTimesheetForm.controls.friday.value},
        {date : date[5],numberOfHours: this.addTimesheetForm.controls.saturday.value},
        {date : date[6],numberOfHours: this.addTimesheetForm.controls.sunday.value}
      ]

      const data ={
        projectId       : 9,
        projectName     : this.addTimesheetForm.controls.project.value,
        activityId      : 15,
        activityName    : this.addTimesheetForm.controls.activity.value,
        status          : 'In Progress',
        remarks         : this.addTimesheetForm.controls.remarks.value,
        timeTaken       : timeTaken
      }
      console.log("Add timesheet data",data);

    }else{
      console.log("this.selectedTimesheet",this.selectedTimesheet);
      let timeTaken = this.selectedTimesheet['timeTaken'];
      timeTaken[0].numberOfHours = this.addTimesheetForm.controls.monday.value;
      timeTaken[1].numberOfHours = this.addTimesheetForm.controls.tuesday.value;
      timeTaken[2].numberOfHours = this.addTimesheetForm.controls.wednesday.value;
      timeTaken[3].numberOfHours = this.addTimesheetForm.controls.thursday.value;
      timeTaken[4].numberOfHours = this.addTimesheetForm.controls.friday.value;
      timeTaken[5].numberOfHours = this.addTimesheetForm.controls.saturday.value;
      timeTaken[6].numberOfHours = this.addTimesheetForm.controls.sunday.value;

      const data = {
      projectId       : 9,
      projectName     : this.addTimesheetForm.controls.project.value,
      activityId      : 15,
      activityName    : this.addTimesheetForm.controls.activity.value, 
      //status          : this.selectedTimesheet.status,
      timeTaken       : timeTaken,
      remarks         : this.addTimesheetForm.controls.remarks.value
      }
      
      console.log("Modify timesheet data",data);

    }

    
  }
  //submitting the timesheet
  onSubmitTimesheet(){
    //remarks =''

    const data = this.selectedTimesheetRow;
    //this.managerId

    
    console.log(this.managerId);
    this.selectedTimesheetRow = [];
    this.submitRemarks ='';
  }
  onselectSupervisor(event:any){
    this.managerId = event.target.value;
  }
  

  
}

