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
  reviewTimesheetArray : any[];
  user:any;
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
    this.getTimesheets();
    // this.reviewTimesheetArray= [
    //   {
    //     ProjectName :"Abc",
    //     EmployeeName :"Jp",
    //     Date      :"2022-05-30T00:00:00",
    //     status   : "submitted"
    //   },
    //   {
    //     ProjectName :"Abc",
    //     EmployeeName :"Jishnu",
    //     Date      :"2022-02-30T00:00:00",
    //     status   : "Approved"
    //   }
    // ]

  }

  //For Getting the timesheet
  private getTimesheets(){
    const pageNo = this.config.currentPage;
    const pageSize = this.config.itemsPerPage;
    const search = this.config.search;
    const url = `${this._url.timesheet.getReviewTimesheet}?PageNumber=${pageNo}&PageSize=${pageSize}&EmployeeID=${this.user.employeeId}`
    this._http.get(url).subscribe(
      {
        next:(res:any)=> {
          this.reviewTimesheetArray = res.data;
          let totalPage = res.totalPages;
          let itemsPerPage = res.pageSize;
          this.config.totalItems = totalPage * itemsPerPage;
        },
        error:(msg) =>{ 
        }
      })

  }


  public onViewTimesheet(item:any){
    //this.router.navigate(['/reviewlink',{ projectId: id, projectName: name, Date: date}]);
    this.router.navigate(['/reviewlink',{ projectId: item.projectId, projectName: item.projectName, date: item.date}]);
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
