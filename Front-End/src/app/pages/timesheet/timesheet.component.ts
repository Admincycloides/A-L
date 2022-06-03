import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
  selectedActivityName: any;
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
        "Date": "2022-05-30 00:00:00.000",
        "EmployeeId": "92S5000000423",
        "EmployeeName": "Emp1",
        "ProjectId": 10,
        "ProjectName": "Project 1",
        "ActivityId": 12,
        "ActivityName": "Activity 1",
        "NumberOfHours": 0.5,
        "Remarks": null,
        "Status": "In Progress",
        "LastUpdatedDate": "2022-05-18 00:00:00.000",
        "LastUpdatedBy": "Emp1",
        "UniqueId": 4
      },
      {
        "Date": "2022-06-03 00:00:00.000",
        "EmployeeId": "92S5000000423",
        "EmployeeName": "Emp1",
        "ProjectId": 10,
        "ProjectName": "Project 2",
        "ActivityId": 13,
        "ActivityName": "Activity 2",
        "NumberOfHours": 0.5,
        "Remarks": "Flagging potato and tomato plots",
        "Status": "In Progress",
        "LastUpdatedDate": "2022-05-19 00:00:00.000",
        "LastUpdatedBy": "Emp1",
        "UniqueId": 5
      }
    ]
    this.getTimesheetDetails();
    this.addTimesheetForm = this._fb.group({
      project: ['', Validators.required],
      activity:['',Validators.required],
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
    this.timeSheetDetails.forEach(
      (item,index)=>{
        let rowArray =[];
        let itemIndex = this.currentWeek.indexOf(moment(item.Date).format("MMMM-DD"));
        rowArray.push(item.Status,item.ProjectName,item.ActivityName);
        while(rowArray.length < 10){
          if(rowArray.length != 10){
            if(rowArray.length == itemIndex+3){
              rowArray.push(item.NumberOfHours);
            }
            else{
              rowArray.push(0);
            }
          }
        }
        // rowArray.splice(itemIndex+2,0,item.NumberOfHours);
        // rowArray.splice(9,0,5);
        rowArray.push(item.NumberOfHours,item.Remarks,item.UniqueId);
         this.timeSheetDetailsArray.push(rowArray);
      }
      
    )
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
      this.selectedProjectName ='';
      this.selectedActivityName = '';
    }
    else{
      this.open(this.content);
      this.selectedProjectName = this.timeSheetDetailsArray[id][1];
      this.selectedActivityName = this.timeSheetDetailsArray[id][2];
      console.log(this.selectedActivityName,this.selectedProjectName);
    }


  }
  onTimesheetDelete(id:any){
    const uniqueId = this.timeSheetDetailsArray[id].UniqueId;
    this._http.delete(`${this._url.timesheet.deleteTimesheet}/${uniqueId}`).subscribe(
    {
      next(res) {
        //this.toastr.success(res.responseMessage)
    }
    });
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
    console.log(this.addTimesheetForm.controls['project'].value);
  }
  selectChanges(value:any){

  }

  
}

