import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from 'app/_services/http.service';
import { UrlService } from 'app/_services/url.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-reviewtimesheet',
  templateUrl: './reviewtimesheet.component.html',
  styleUrls: ['./reviewtimesheet.component.scss']
})
export class ReviewtimesheetComponent implements OnInit {
  reviewTimesheetArray: any[];
  user:any;
  caption:any;
  searchTerm :any;
  public config = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 1,
    search:''
  };

  constructor(private router: Router,
    private toast: ToastrService,
    private _url: UrlService,
    private _http: HttpService,
    private activatedRoute:ActivatedRoute) { }

  ngOnInit(): void {
    this.user = JSON.parse(localStorage.getItem('user'));
    this.caption = "Review Timesheet"
    //this.getTimesheets();
    this.reviewTimesheetArray= [
      {
        ProjectName :'Abc',
        EmployeeName :'Jishnu',
        Date      :'2022-05-30T00:00:00',
        status   : 'submitted'
      },
      {
        ProjectName :'Abc',
        EmployeeName :'Jishnu',
        Date      :'2022-02-30T00:00:00',
        status   : 'Approved'
      }
    ]

    this.reviewTimesheetArray.forEach((item)=>{
      console.log(item.ProjectName)
      console.log(item.EmployeeName)
      console.log(item.Date)
      console.log(item.status)
    })
  }


  //For Getting the timesheet
  private getTimesheets(){
    // const url = `${this._url.review.getReviewTimesheet}?EmployeeID=${this.user.employeeId}`
    // this._http.get(url).subscribe(
    //   {
    //     next:(res:any)=> {
    //       this.reviewTimesheetArray = res.data
    //     },
    //     error:(msg) =>{ 
    //     }
    //   })

  }


  public onViewTimesheet(item:any,name:any,id:any,date:any){
    //,item.ProjectName,item.Date
    //this.router.navigate(['/reviewlink',{ projectId: item.ProjectId, projectName: item.ProjectName, Date: item.date}]);
    this.router.navigate(['/reviewlink',{ projectId: id, projectName: name, date: date}],{relativeTo:this.activatedRoute});
  }
  public pageChanged(event:any){
    this.config.currentPage =event;
    this.getTimesheets();
  }
   //To serach project
   searchItems(event:any){
    this.config.search = event.target.value;
    this.searchTerm = event.target.value;
    this.config.currentPage = 1;
    this.getTimesheets();
  }
  // To clear search
  public clearSearch() {
    this.searchTerm = '';
    this.config.currentPage = 1;
    this.config.search = '';
    this.getTimesheets();
  }

}
