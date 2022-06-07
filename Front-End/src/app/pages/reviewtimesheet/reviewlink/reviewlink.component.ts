import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from 'app/_services/http.service';
import { UrlService } from 'app/_services/url.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-reviewlink',
  templateUrl: './reviewlink.component.html',
  styleUrls: ['./reviewlink.component.scss']
})
export class ReviewlinkComponent implements OnInit {
  projectIid: any;
  projectName :any;
  submitDate : any;
  user: any;
  timesheetDetails: any[];
  timesheetDates: any[];
  searchTerm :any;
  submitRemarks: any;
  public config = {
    id: 'timesheet',
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
      console.log(params);
      this.projectIid = params.params.projectId;
      this.projectName = params.params.projectName;
      this.submitDate =params.params.date
    });
    this.user = JSON.parse(localStorage.getItem('user'));
    this.getReviewTimesheetDetails()
  }

  //For getting timesheet details
  private getReviewTimesheetDetails(){
    const url = `${this._url.review.getReviewTimesheetDetails}?`
    const body = {
      EmployeeId : this.user.userId,
      ProjectId  : this.projectIid,
      Date :this.submitDate

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
    // const remarks = remarks;
    // if(value === 'approve'){
    //   console.log(value);
    // }else{
    //   console.log(value);
    // }
    const url = `${this._url.review.supervisorDecision}?Action=${data}`
    const body = this.timesheetDetails;
    body.forEach((item)=>{
      item.supervisorRemarks = remarks;
    })
      this._http.post(url,body).subscribe(
        {
          next:(res:any)=> {
            this.toast.success("Successfully "+value+" timesheet");
          },
          error:(error) =>{  
          }
        });

    this.router.navigate(['/review']);
  }
  pageChanged(event:any){
    this.config.currentPage = event;
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
