import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from 'app/_services/http.service';
import { UrlService } from 'app/_services/url.service';
import * as moment from 'moment';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-reviewlink',
  templateUrl: './reviewlink.component.html',
  styleUrls: ['./reviewlink.component.scss']
})
export class ReviewlinkComponent implements OnInit {
  projectId: any;
  projectName :any;
  submitDate : any;
  user: any;
  timesheetDetails: any[];
  timesheetDates: any[];
  searchTerm :any;
  submitRemarks: any;
  public config = {
    id: 'timesheetDetails',
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 1,
    search:''
  };

  constructor(private router: Router,private activedRouter :ActivatedRoute,
    private toast: ToastrService,
    private _url: UrlService,
    private _http: HttpService) { }

  ngOnInit(): void {
    this.activedRouter.paramMap.subscribe((params: any) => {
      console.log("params",params);
      this.projectId = parseInt(params.params.projectId);
      this.projectName = params.params.projectName;
      this.submitDate =params.params.date
    });
    this.user = JSON.parse(localStorage.getItem('user'));
    this.getReviewTimesheetDetails()


  //   this.timesheetDetails =[
  //     {
  //         "projectId": 1,
  //         "projectName": null,
  //         "activityId": 6,
  //         "activityName": 'activity',
  //         "status": "In Progress",
  //         "remarks": "None",
  //         "employeeRemarks": null,
  //         "supervisorRemarks": null,
  //         "timeTaken": [
  //             {
  //                 "date": "2022-06-06T00:00:00",
  //                 "numberOfHours": 2,
  //                 "uniqueId": 27
  //             },
  //             {
  //                 "date": "2022-06-07T00:00:00",
  //                 "numberOfHours": 2,
  //                 "uniqueId": 28
  //             },
  //             {
  //                 "date": "2022-06-08T00:00:00",
  //                 "numberOfHours": 2,
  //                 "uniqueId": 29
  //             },
  //             {
  //                 "date": "2022-06-09T00:00:00",
  //                 "numberOfHours": 2,
  //                 "uniqueId": 30
  //             },
  //             {
  //                 "date": "2022-06-10T00:00:00",
  //                 "numberOfHours": 2,
  //                 "uniqueId": 31
  //             },
  //             {
  //                 "date": "2022-06-11T00:00:00",
  //                 "numberOfHours": 0,
  //                 "uniqueId": 0
  //             },
  //             {
  //                 "date": "2022-06-12T00:00:00",
  //                 "numberOfHours": 0,
  //                 "uniqueId": 0
  //             }
  //         ]
  //     },
  //     {
  //         "projectId": 1,
  //         "projectName": null,
  //         "activityId": 7,
  //         "activityName": 'activity',
  //         "status": "In Progress",
  //         "remarks": "None",
  //         "employeeRemarks": null,
  //         "supervisorRemarks": null,
  //         "timeTaken": [
  //             {
  //                 "date": "2022-06-06T00:00:00",
  //                 "numberOfHours": 2,
  //                 "uniqueId": 27
  //             },
  //             {
  //                 "date": "2022-06-07T00:00:00",
  //                 "numberOfHours": 2,
  //                 "uniqueId": 28
  //             },
  //             {
  //                 "date": "2022-06-08T00:00:00",
  //                 "numberOfHours": 2,
  //                 "uniqueId": 29
  //             },
  //             {
  //                 "date": "2022-06-09T00:00:00",
  //                 "numberOfHours": 2,
  //                 "uniqueId": 30
  //             },
  //             {
  //                 "date": "2022-06-10T00:00:00",
  //                 "numberOfHours": 2,
  //                 "uniqueId": 31
  //             },
  //             {
  //                 "date": "2022-06-11T00:00:00",
  //                 "numberOfHours": 0,
  //                 "uniqueId": 0
  //             },
  //             {
  //                 "date": "2022-06-12T00:00:00",
  //                 "numberOfHours": 0,
  //                 "uniqueId": 0
  //             }
  //         ]
  //     }
  // ]

  //this.timesheetDates = this.timesheetDetails[0].timeTaken
  }

  //For getting timesheet details
  private getReviewTimesheetDetails(){
    const pageNo = this.config.currentPage;
    const pageSize = this.config.itemsPerPage;
    const search = this.config.search;
    //const date = moment(this.submitDate).utc().format();
    console.log(moment().utc(this.submitDate).format());
    const date = this.submitDate;
    const url = `${this._url.timesheet.getReviewTimesheetDetails}?PageNumber=${pageNo}&PageSize=${pageSize}&search=${search}`
    const body = {
      employeeId: this.user.employeeId,
      projectId : this.projectId,
      date : date
    }
      this._http.post(url,body).subscribe(
        {
          next:(res:any)=> {
           this.timesheetDetails = res.data;
           this.timesheetDates = res.data[0].timeTaken
          },
          error:(error) =>{  
          }
        });
  }

  //on clicking approve or disapprove
  public onAcceptingTimesheet(value:string,remarks:any){
    const data = value;
    console.log(remarks);
    console.log(this.timesheetDates);
    const url = `${this._url.timesheet.supervisorDecision}?SupervisorID=${this.user.employeeId}&Action=${data}`
    const body = this.timesheetDetails;
    body.forEach((item)=>{
      item.supervisorRemarks = remarks;
    })
      this._http.post(url,body).subscribe(
        {
          next:(res:any)=> {
            this.toast.success("Successfully "+value+" timesheet");
            this.router.navigate(['/review']);
          },
          error:(error) =>{  
          }
        });
  }
  pageChanged(event:any){
    this.config.currentPage = event;
    this.getReviewTimesheetDetails();
  }
  //To serach activity
  searchItems(event:any){
    this.config.search = event.target.value;
    this.searchTerm = event.target.value;
    this.config.currentPage = 1;
    this.getReviewTimesheetDetails();
  }
  // To clear search
  public clearSearch() {
    this.searchTerm = '';
    this.config.currentPage = 1;
    this.config.search = '';
    this.getReviewTimesheetDetails();
  }

}
