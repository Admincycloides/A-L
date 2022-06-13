import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpService } from "app/_services/http.service";
import { UrlService } from "app/_services/url.service";
import * as moment from "moment";
import { ToastrService } from "ngx-toastr";

@Component({
  selector: "app-reviewlink",
  templateUrl: "./reviewlink.component.html",
  styleUrls: ["./reviewlink.component.scss"],
})
export class ReviewlinkComponent implements OnInit {
  projectId: any;
  projectName: any;
  submitDate: any;
  user: any;
  timesheetDetails: any[];
  timesheetDates: any[];
  searchTerm: any;
  submitRemarks: any;
  status: any;
  empID: any;
  public config = {
    id: "timesheetDetails",
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 1,
    search: "",
  };

  constructor(
    private router: Router,
    private activedRouter: ActivatedRoute,
    private toast: ToastrService,
    private _url: UrlService,
    private _http: HttpService
  ) {}

  ngOnInit(): void {
    this.activedRouter.paramMap.subscribe((params: any) => {
      this.projectId = parseInt(params.params.projectId);
      this.projectName = params.params.projectName;
      this.submitDate = params.params.date;
      this.status = params.params.status;
      this.empID = params.params.empID;
    });
    this.user = JSON.parse(localStorage.getItem("user"));
    this.getReviewTimesheetDetails();
  }

  //For getting timesheet details
  private getReviewTimesheetDetails() {
    const pageNo = this.config.currentPage;
    const pageSize = this.config.itemsPerPage;
    const search = this.config.search;
    //const date = moment(this.submitDate).utc().format();
    const date = this.submitDate;
    const url = `${this._url.timesheet.getReviewTimesheetDetails}?PageNumber=${pageNo}&PageSize=${pageSize}&search=${search}`;
    const body = {
      employeeId: this.user.employeeId,
      projectId: this.projectId,
      date: date,
    };
    this._http.post(url, body).subscribe({
      next: (res: any) => {
        this.timesheetDetails = res.data;
        this.timesheetDates = res.data[0].timeTaken;
      },
      error: (error) => {},
    });
  }

  //on clicking approve or disapprove
  public onAcceptingTimesheet(value: string, remarks: any) {
    const data = value;
    const url = `${this._url.timesheet.supervisorDecision}?SupervisorID=${this.user.employeeId}&EmployeeID=${this.empID}&Action=${data}`;
    const body = this.timesheetDetails;
    body.forEach((item) => {
      item.supervisorRemarks = remarks;
    });
    this._http.post(url, body).subscribe({
      next: (res: any) => {
        this.toast.success("Successfully " + value + " timesheet");
        this.router.navigate(["/review"]);
      },
      error: (error) => {},
    });
  }
  pageChanged(event: any) {
    this.config.currentPage = event;
    this.getReviewTimesheetDetails();
  }
  //To serach activity
  searchItems(event: any) {
    this.config.search = event.target.value;
    this.searchTerm = event.target.value;
    this.config.currentPage = 1;
    this.getReviewTimesheetDetails();
  }
  // To clear search
  public clearSearch() {
    this.searchTerm = "";
    this.config.currentPage = 1;
    this.config.search = "";
    this.getReviewTimesheetDetails();
  }
}
