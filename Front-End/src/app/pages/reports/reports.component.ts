import { Component, OnInit } from "@angular/core";
import { HttpService } from "app/_services/http.service";
import { UrlService } from "app/_services/url.service";
import * as moment from "moment";
import { IDropdownSettings } from "ng-multiselect-dropdown";
import { ToastrService } from "ngx-toastr";

@Component({
  selector: "app-reports",
  templateUrl: "./reports.component.html",
  styleUrls: ["./reports.component.scss"],
})
export class ReportsComponent implements OnInit {
  dropdownSettings: IDropdownSettings = {};
  dropdownEmployeeSettings: IDropdownSettings = {};
  projectList: any[];
  employeeList: any[];
  maxPickerDateTo: any;
  minPickerDateTo: any;
  startDate: any;
  endDate: any;
  selectedProjectList = [];
  selectedEmployeeList = [];
  reportList: any[];
  allDates: any[];
  totalHours: any[];

  constructor(
    private _url: UrlService,
    private _http: HttpService,
    private toast: ToastrService
  ) {}

  ngOnInit(): void {
    this.dropdownSettings = {
      singleSelection: false,
      idField: "projectId",
      textField: "projectName",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 3,
      allowSearchFilter: true,
    };
    this.dropdownEmployeeSettings = {
      singleSelection: false,
      idField: "employeeId",
      textField: "employeeName",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 3,
      allowSearchFilter: true,
    };
    this.getProjectList();
    this.getEmployeeList();
  }
  private getProjectList() {
    const url = `${this._url.project.getProjectList}`;
    this._http.get(url).subscribe({
      next: (res: any) => {
        this.projectList = res.data;
      },
      error: (msg) => {},
    });
  }
  private getEmployeeList() {
    const url = `${this._url.Employee.getAllEmployeeList}`;
    this._http.get(url).subscribe({
      next: (res: any) => {
        this.employeeList = res.data;
      },
      error: (msg) => {},
    });
  }
  public onProjectEmployeeDeSelect(item: any, value: any) {
    if (value === "project") {
      this.selectedProjectList.splice(
        this.selectedProjectList.indexOf(item),
        1
      );
    } else {
      this.selectedEmployeeList.splice(
        this.selectedEmployeeList.indexOf(item),
        1
      );
    }
    console.log(this.selectedProjectList);
  }
  public onProjectEmployeeSelect(item: any, value: any) {
    if (value === "project") {
      this.selectedProjectList.push(item);
    } else {
      this.selectedEmployeeList.push(item);
    }
  }
  public onProjectEmployeeSelectAll(item: any, value: any) {
    if (value === "project") this.selectedEmployeeList = this.employeeList;
    if (value === "employee") this.selectedProjectList = this.projectList;
  }
  public onProjectEmployeeDeSelectAll(item: any, value: any) {
    if (value === "project") this.selectedEmployeeList = [];
    if (value === "employee") this.selectedProjectList = [];
  }
  public dateChange(event: any, value: any) {
    if (value === "from") {
      this.maxPickerDateTo = {
        year: event.year + 1,
        month: event.month,
        day: event.day,
      };
      this.minPickerDateTo = event;
      const date = new Date();
      date.setFullYear(event.year);
      date.setMonth(event.month - 1);
      date.setDate(event.day);
      this.startDate = date;
    } else {
      const date = new Date();
      date.setFullYear(event.year);
      date.setMonth(event.month - 1);
      date.setDate(event.day);
      this.endDate = date;
    }
  }
  private dateFormatter(start: any, end: any) {
    var dateArray = [];
    var currentDate = moment(start);
    var stopDate = moment(end);
    while (
      moment(currentDate).format("YYYY-MM-DD 00:00:00.000") <=
      moment(stopDate).format("YYYY-MM-DD 00:00:00.000")
    ) {
      //dateArray.push(moment(currentDate).format("MMMM-DD"));
      dateArray.push(currentDate);
      currentDate = moment(currentDate).add(1, "days");
    }
    return dateArray;
  }

  //To get total hour spent to a project by a employee
  public getTotalHours(item: any) {
    let total = 0;
    item.forEach((el) => {
      total = total + el.numberOfHours;
    });
    return total;
  }
  //To check whether all 4 fields are selected or not
  private setStatus() {
    if (
      this.selectedEmployeeList?.length &&
      this.selectedProjectList?.length &&
      this.startDate &&
      this.endDate
    )
      return true;
    else return false;
  }

  //To Genearet Report
  public onGenerateReport() {
    if (this.setStatus()) {
      if (this.endDate.getFullYear() - this.startDate.getFullYear() <= 1) {
        const pList = [];
        const eList = [];
        this.allDates = this.dateFormatter(this.startDate, this.endDate);
        console.log(this.allDates);
        const sDate = moment(this.startDate).format("YYYY-MM-DD 00:00:00.000");
        const eDate = moment(this.endDate).format("YYYY-MM-DD 00:00:00.000");
        this.selectedProjectList.forEach((item) => {
          pList.push(item.projectId);
        });
        this.selectedEmployeeList.forEach((item) => {
          eList.push(item.employeeId);
        });

        const body = {
          projectIds: pList,
          employeeId: eList,
          fromDate: sDate,
          toDate: eDate,
        };
        const url = `${this._url.timesheet.getTimesheetReport}`;
        this._http.post(url, body).subscribe({
          next: (res: any) => {
            this.reportList = res.reportViewModels;
            this.totalHours = res.reportDayWiseTotals;
          },
        });
      } else {
        this.toast.error("Please select date with in one year!!");
      }
    } else {
      this.toast.error("All Fields Are Mandatory!!");
    }
  }


  public getValues(date: any, list: any[]) {
    let value = [];
    value = list.filter((item) => {
      return item.date == moment(date._d).format("YYYY-MM-DDT00:00:00");
    });
    console.log(value);
    if (value.length) return value[0]?.numberOfHours;
    else null;
  }

  //to get total hours of date
  public getTotalHoursValues(date: any) {
    let value = [];
    value = this.totalHours.filter((item) => {
      return item.date == moment(date._d).format("YYYY-MM-DDT00:00:00");
    });
    console.log(value);
    if (value.length) return value[0]?.numberOfHours;
    else null;
  }
}
