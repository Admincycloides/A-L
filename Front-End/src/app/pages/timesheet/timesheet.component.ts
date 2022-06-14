import { Component, ElementRef, OnInit, ViewChild } from "@angular/core";
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from "@angular/forms";
import {
  ModalDismissReasons,
  NgbModal,
  NgbModalRef,
} from "@ng-bootstrap/ng-bootstrap";
import { HttpService } from "app/_services/http.service";
import { UrlService } from "app/_services/url.service";
import * as moment from "moment";
import { ToastrService } from "ngx-toastr";

@Component({
  selector: "app-timesheet",
  templateUrl: "./timesheet.component.html",
  moduleId: module.id,
  styleUrls: ["./timesheet.component.scss"],
})
export class TimesheetComponent implements OnInit {
  @ViewChild("content") content: any;
  closeResult = "";
  userDetails: any;
  startOfWeek: any;
  endOfWeek: any;
  private modalRef: NgbModalRef;

  timeSheetDetails: any[];
  currentWeek: any[];
  public config = {
    id: "timesheet",
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 1,
  };
  formSubmitted = false;
  timeSheetDetailsArray = [];
  model;
  weekShow: any;
  selectAllTimesheet: boolean = false;
  addTimesheetForm: FormGroup;
  projectList: any[];
  activityList: any[];
  selectedProjectName: any;
  selectedDates: any[];
  selectedTimesheet = {};
  superVisorList = [];
  selectedTimesheetRow = [];
  managerId: any;
  timesheetRemarks: any;
  submitRemarks: any;
  showSideWindow = false;
  showSideWindowCheckAll = false;

  get f() {
    return this.addTimesheetForm.controls;
  }

  constructor(
    private _url: UrlService,
    private _http: HttpService,
    private modalService: NgbModal,
    private _fb: FormBuilder,
    private toast: ToastrService
  ) {}

  ngOnInit(): void {
    this.userDetails = JSON.parse(localStorage.getItem("user"));
    this.startOfWeek = moment().startOf("isoWeek").toDate();
    this.endOfWeek = moment().endOf("isoWeek").toDate();
    this.weekShow =
      moment(this.startOfWeek).format("MMMM-DD") +
      "-" +
      moment(this.endOfWeek).format("MMMM-DD");
    this.currentWeek = this.dateFormatter(
      moment(this.startOfWeek).format("YYYY-MM-DD"),
      moment(this.endOfWeek).format("YYYY-MM-DD")
    );
    this.getTimesheetDetails(this.startOfWeek, this.endOfWeek);
    this.getSupervisorDetails();
    this.getProjectActivityDetails();

    this.addTimesheetForm = this._fb.group({
      project: ["", Validators.required],
      activity: ["", Validators.required],
      monday: [0.0,[Validators.min(0),Validators.max(24)]],
      tuesday: [0.0,[Validators.min(0),Validators.max(24)]],
      wednesday: [0.0,[Validators.min(0),Validators.max(24)]],
      thursday: [0.0,[Validators.min(0),Validators.max(24)]],
      friday: [0.0,[Validators.min(0),Validators.max(24)]],
      saturday: [0.0,[Validators.min(0),Validators.max(24)]],
      sunday: [0.0,[Validators.min(0),Validators.max(24)]],
      remarks: ["",[Validators.min(0),Validators.max(24)]],
    });
  }
  //To fetch supervisors
  private getSupervisorDetails() {
    const url = `${this._url.Employee.getSupervisorDetails}`;
    this._http.get(url).subscribe({
      next: (res: any) => {
        this.superVisorList = res.data;
      },
    });
  }

  //To fetch project and corresponding activities of employee
  private getProjectActivityDetails() {
    const url = `${this._url.project.getprojectListbyEmployeeID}?EmployeeID=${this.userDetails.employeeId}`;
    this._http.get(url).subscribe({
      next: (res: any) => {
        this.projectList = res.data;
      },
    });
  }

  //when selecting project during entering timesheet.
  public onProjectSelect(event: any) {
    const project = event.target.value.split(":")[1].trim();
    if (project === "Select Project") this.activityList = [];
    else {
      this.projectList.forEach((item) => {
        if (item.projectName == project) {
          this.activityList = item.activities;
        }
      });
    }
  }

  public pageChanged(event) {
    this.config.currentPage = event;
    this.timeSheetDetails = [];
    this.getTimesheetDetails(this.startOfWeek, this.endOfWeek);
  }
  // for viewing timsheet
  private getTimesheetDetails(start: any, end: any) {
    const fromDate = moment(start).format("YYYY-MM-DD 00:00:00.000");
    const toDate = moment(end).format("YYYY-MM-DD 00:00:00.000");
    const date = moment.utc().format();
    const pageNo = this.config.currentPage;
    const pageSize = this.config.itemsPerPage;
    const body = {
      employeeId: this.userDetails.employeeId,
      fromDate: fromDate,
      toDate: toDate,
    };

    const url = `${this._url.timesheet.getTimesheet}?PageNumber=${pageNo}&PageSize=${pageSize}`;
    this._http.post(url, body).subscribe({
      next: (res: any) => {
        //this.timeSheetDetails = res.timesheetDetails;
        this.timeSheetDetails = res.data;
        let totalPage = res.totalPages;
        let itemsPerPage = res.pageSize;
        this.config.totalItems = totalPage * itemsPerPage;
      },
    });
  }
  dateChange(event) {
    this.selectedTimesheetRow = [];
    const date = event.month + "-" + event.day + "-" + event.year;
    this.startOfWeek = moment(date).startOf("isoWeek").toDate();
    this.endOfWeek = moment(date).endOf("isoWeek").toDate();
    this.model = "2022-12-5";
    this.currentWeek = this.dateFormatter(
      moment(this.startOfWeek).format("YYYY-MM-DD"),
      moment(this.endOfWeek).format("YYYY-MM-DD")
    );
    this.weekShow =
      moment(this.startOfWeek).format("MMMM-DD") +
      "-" +
      moment(this.endOfWeek).format("MMMM-DD");
    this.getTimesheetDetails(this.startOfWeek, this.endOfWeek);
  }
  private dateFormatter(start: any, end: any) {
    var dateArray = [];
    var currentDate = moment(start);
    var stopDate = moment(end);
    while (currentDate <= stopDate) {
      //dateArray.push(moment(currentDate).format("MMMM-DD"));
      dateArray.push(currentDate);
      currentDate = moment(currentDate).add(1, "days");
    }
    return dateArray;
  }
  onPreviousClick() {
    this.selectedTimesheetRow = [];
    this.timeSheetDetails = [];
    this.startOfWeek = moment(this.startOfWeek).subtract(1, "weeks");
    this.endOfWeek = moment(this.endOfWeek).subtract(1, "weeks");
    this.currentWeek = this.dateFormatter(
      moment(this.startOfWeek).format("YYYY-MM-DD"),
      moment(this.endOfWeek).format("YYYY-MM-DD")
    );
    this.weekShow =
      moment(this.startOfWeek).format("MMMM-DD") +
      "-" +
      moment(this.endOfWeek).format("MMMM-DD");
    this.getTimesheetDetails(this.startOfWeek, this.endOfWeek);
  }
  onNextClick() {
    this.selectedTimesheetRow = [];
    this.timeSheetDetails = [];
    this.startOfWeek = moment(this.startOfWeek).add(1, "weeks");
    this.endOfWeek = moment(this.endOfWeek).add(1, "weeks");
    this.currentWeek = this.dateFormatter(
      moment(this.startOfWeek).format("YYYY-MM-DD"),
      moment(this.endOfWeek).format("YYYY-MM-DD")
    );
    this.weekShow =
      moment(this.startOfWeek).format("MMMM-DD") +
      "-" +
      moment(this.endOfWeek).format("MMMM-DD");
    this.getTimesheetDetails(this.startOfWeek, this.endOfWeek);
  }

  public open(content: any) {
    this.modalRef = this.modalService.open(content, {
      ariaLabelledBy: "modal-basic-title",
      size: "xl",
    });
    this.modalRef.result.then(
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
      return "by pressing ESC";
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return "by clicking on a backdrop";
    } else {
      return `with: ${reason}`;
    }
  }

  //Timesheet Editing and Add
  onTimesheetAddEdit(id: any) {
    this.open(this.content);
    if (id == -1) {
      this.addTimesheetForm.setValue({
        project: "",
        activity: "",
        monday: 0.0,
        tuesday: 0.0,
        wednesday: 0.0,
        thursday: 0.0,
        friday: 0.0,
        saturday: 0.0,
        sunday: 0.0,
        remarks: "",
      });
      this.selectedTimesheet = {};
    } else {
      this.addTimesheetForm.controls.remarks.setValue(
        this.timeSheetDetails[id].remarks
      );
      this.addTimesheetForm.controls.project.setValue(
        this.timeSheetDetails[id].projectName
      );
      this.addTimesheetForm.controls.activity.setValue(
        this.timeSheetDetails[id].activityName
      );
      this.addTimesheetForm.controls.monday.setValue(
        this.timeSheetDetails[id].timeTaken[0].numberOfHours
      );
      this.addTimesheetForm.controls.tuesday.setValue(
        this.timeSheetDetails[id].timeTaken[1].numberOfHours
      );
      this.addTimesheetForm.controls.wednesday.setValue(
        this.timeSheetDetails[id].timeTaken[2].numberOfHours
      );
      this.addTimesheetForm.controls.thursday.setValue(
        this.timeSheetDetails[id].timeTaken[3].numberOfHours
      );
      this.addTimesheetForm.controls.friday.setValue(
        this.timeSheetDetails[id].timeTaken[4].numberOfHours
      );
      this.addTimesheetForm.controls.saturday.setValue(
        this.timeSheetDetails[id].timeTaken[5].numberOfHours
      );
      this.addTimesheetForm.controls.sunday.setValue(
        this.timeSheetDetails[id].timeTaken[6].numberOfHours
      );
      this.projectList.forEach((item) => {
        if (item.projectName == this.addTimesheetForm.controls.project.value) {
          this.activityList = item.activities;
        }
      });

      this.selectedTimesheet = this.timeSheetDetails[id];
    }
  }
  // Deleting Timesheet row
  onTimesheetDelete(id: any) {
    const body = this.timeSheetDetails[id];
    const url = `${this._url.timesheet.deleteTimesheet}`;
    this._http.post(url, body).subscribe({
      next: (res: any) => {
        this.getTimesheetDetails(this.startOfWeek, this.endOfWeek);
        this.toast.success("Successfully Deleted");
      },
    });
  }

  //on checking checkbox of the table
  onRowCheck(id: any) {
    if (id == -1) {
      this.selectedTimesheetRow = [];
      this.showSideWindowCheckAll = !this.showSideWindowCheckAll;
      this.showSideWindow = false;
      this.selectAllTimesheet = !this.selectAllTimesheet;
      this.timeSheetDetails.forEach((item) => {
        if (item.status == "In Progress" || item.status == "Rejected") {
          if (!this.selectedTimesheetRow.includes(item)) {
            this.selectedTimesheetRow.push(item);
          }
        }
      });
    } else {
      this.showSideWindow = !this.showSideWindow;
      this.showSideWindowCheckAll = false;
      if (!this.selectedTimesheetRow.includes(this.timeSheetDetails[id])) {
        this.selectedTimesheetRow.push(this.timeSheetDetails[id]);
      } else {
        this.selectedTimesheetRow.splice(
          this.selectedTimesheetRow.indexOf(this.timeSheetDetails[id]),
          1
        );
      }
    }
  }
  //saving the timsheet details
  onSaveTimesheetDetails() {
    this.formSubmitted = true;
    if (this.addTimesheetForm.valid) {
      let projectId: any;
      let activityId: any;
      this.formSubmitted = false;
      this.projectList.forEach((item) => {
        if (item.projectName == this.addTimesheetForm.controls.project.value) {
          projectId = item.projectId;
          item.activities.forEach((el) => {
            if (
              el.activityName == this.addTimesheetForm.controls.activity.value
            )
              activityId = el.activityId;
          });
        }
      });
      if (!Object.keys(this.selectedTimesheet).length) {
        let date = [];
        this.currentWeek.forEach((day) => {
          date.push(moment(day).format("YYYY-MM-DDT00:00:00"));
        });
        const timeTaken = [
          {
            date: date[0],
            numberOfHours: parseFloat(
              this.addTimesheetForm.controls.monday.value
            ),
          },
          {
            date: date[1],
            numberOfHours: parseFloat(
              this.addTimesheetForm.controls.tuesday.value
            ),
          },
          {
            date: date[2],
            numberOfHours: parseFloat(
              this.addTimesheetForm.controls.wednesday.value
            ),
          },
          {
            date: date[3],
            numberOfHours: parseFloat(
              this.addTimesheetForm.controls.thursday.value
            ),
          },
          {
            date: date[4],
            numberOfHours: parseFloat(
              this.addTimesheetForm.controls.friday.value
            ),
          },
          {
            date: date[5],
            numberOfHours: parseFloat(
              this.addTimesheetForm.controls.saturday.value
            ),
          },
          {
            date: date[6],
            numberOfHours: parseFloat(
              this.addTimesheetForm.controls.sunday.value
            ),
          },
        ];
        const body = {
          projectId: projectId,
          projectName: this.addTimesheetForm.controls.project.value,
          activityId: activityId,
          activityName: this.addTimesheetForm.controls.activity.value,
          status: "In Progress",
          remarks: this.addTimesheetForm.controls.remarks.value,
          timeTaken: timeTaken,
        };
        const url = `${this._url.timesheet.addTimesheet}?EmployeeId=${this.userDetails.employeeId}`;
        this._http.post(url, body).subscribe({
          next: (res: any) => {
            this.toast.success(res.responseMessage);
            this.getTimesheetDetails(this.startOfWeek, this.endOfWeek);
            this.activityList = [];
            this.modalRef.close();
          },
          error: (err: any) => {
            this.toast.error(err.error.responseMessage);
          },
        });
      } else {
        let timeTaken = this.selectedTimesheet["timeTaken"];
        timeTaken[0].numberOfHours = parseFloat(
          this.addTimesheetForm.controls.monday.value
        );
        timeTaken[1].numberOfHours = parseFloat(
          this.addTimesheetForm.controls.tuesday.value
        );
        timeTaken[2].numberOfHours = parseFloat(
          this.addTimesheetForm.controls.wednesday.value
        );
        timeTaken[3].numberOfHours = parseFloat(
          this.addTimesheetForm.controls.thursday.value
        );
        timeTaken[4].numberOfHours = parseFloat(
          this.addTimesheetForm.controls.friday.value
        );
        timeTaken[5].numberOfHours = parseFloat(
          this.addTimesheetForm.controls.saturday.value
        );
        timeTaken[6].numberOfHours = parseFloat(
          this.addTimesheetForm.controls.sunday.value
        );

        const body = {
          projectId: projectId,
          projectName: this.addTimesheetForm.controls.project.value,
          activityId: activityId,
          activityName: this.addTimesheetForm.controls.activity.value,
          //status          : 'In Progress',
          timeTaken: timeTaken,
          remarks: this.addTimesheetForm.controls.remarks.value,
        };

        const url = `${this._url.timesheet.editTimesheet}`;
        this._http.post(url, body).subscribe({
          next: (res: any) => {
            this.toast.success(res.responseMessage);
            this.getTimesheetDetails(this.startOfWeek, this.endOfWeek);
            this.modalRef.close();
          },
          error: (err: any) => {
            this.toast.error(err.responseMessage);
          },
        });
      }
    }
  }
  //submitting the timesheet
  onSubmitTimesheet() {
    //console.log(this.selectedTimesheetRow);
    if (this.selectedTimesheetRow.length != 0) {
      if (this.managerId && this.managerId != "(Supervisor)") {
        this.selectedTimesheetRow.forEach((item) => {
          item["employeeRemarks"] = this.submitRemarks;
        });
        const body = this.selectedTimesheetRow;
        const url = `${this._url.timesheet.submitTimesheet}?ManagerID=${this.managerId}&EmployeeID=${this.userDetails.employeeId}`;
        this._http.post(url, body).subscribe({
          next: (res: any) => {
            this.toast.success(res.responseMessage);
            this.selectedTimesheetRow = [];
            this.submitRemarks = "";
            this.getTimesheetDetails(this.startOfWeek, this.endOfWeek);
          },
        });
      } else {
        this.toast.error("Please Select Your Supervisor.");
      }
    } else {
      this.toast.error(
        "Please select the items you wish to apply an action to."
      );
    }
  }

  onselectSupervisor(event: any) {
    this.managerId = event.target.value;
  }
}
