import { Component, OnInit } from '@angular/core';
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
  userDetails: any;
  startOfWeek: any;
  endOfWeek: any;


  constructor(private _url: UrlService,
    private _http: HttpService) { }

  ngOnInit(): void {
    this.userDetails = JSON.parse(localStorage.getItem('token'));
    this.getEmployeeDetails();
    this.startOfWeek = moment().startOf('isoWeek').toDate();
    this.endOfWeek = moment().endOf('isoWeek').toDate();
  }
  private getEmployeeDetails(){

    this._http.get(`${this._url.login.getEmployeeDetails}/${this.userDetails.userId}`).subscribe(
      {
        next(res) {
          //localStorage.setItem('user',JSON.stringify(res.data));
        }
      }
    )
  }

}
