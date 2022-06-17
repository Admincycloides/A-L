import { Component, OnInit } from '@angular/core';
import { HttpService } from 'app/_services/http.service';
import { UrlService } from 'app/_services/url.service';
import { NgbModal,NgbModalConfig } from '@ng-bootstrap/ng-bootstrap';
import { IDropdownSettings } from "ng-multiselect-dropdown";
import { Tooltip } from 'chart.js';
import * as moment from "moment";
import { ToastrService } from 'ngx-toastr';
import { TouchSequence } from 'selenium-webdriver';


@Component({
  selector: 'app-log',
  templateUrl: './log.component.html',
  styleUrls: ['./log.component.scss'],
  providers: [NgbModalConfig, NgbModal]
})
export class LogComponent implements OnInit {

  public LogAudits = [];
  tooltipName:any;
  startOfWeek: any;
  endOfWeek: any;
  searchTerm: any= '';
  FromDate:any;
  ToDate:any;
  user:any;
  Logs:any[];
  supervisorFlag:string;
  tooltipDescription:any;
  public activity:any;
  public action:any;
  dropdownEmployeeSettings: IDropdownSettings = {};

  public config = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 1,
    search: "",
  };

  constructor(private _http: HttpService,private _url: UrlService,config: NgbModalConfig, private modalService: NgbModal, private _toast:ToastrService) { 
    config.backdrop = 'static';
    config.keyboard = false;
  }
  open(content) {
    this.modalService.open(content);
  }
  ngOnInit(): void {

    this.user = JSON.parse(localStorage.getItem('user'));
    this.supervisorFlag = this.user.supervisorFlag;
    this.Giveaudittrail()

    // this.getAuditLogs();
  }


  // private getAuditLogs(){
  //   const url = `${this._url.audit.auditlog}`
  //   this._http.get(url).subscribe({
  //     next:(res:any)=>{
  //         this.LogAudits = res.data;
  //         console.log("hello",this.LogAudits);
  //     }
  //       })
  //   }

    public getName(value:any){

      let newString;
      if(value.length > 100 ){
      newString = value.substring(0,80)+'....';
    }
  return newString
}

public Giveaudittrail() {
  const search = this.config.search;
  const pageNo = this.config.currentPage;
  const pageSize = this.config.itemsPerPage;
  const fromDate = this.FromDate ? moment(this.FromDate).format("YYYY-MM-DD 00:00:00.000"):""
  const toDate = this.ToDate ? moment(this.ToDate).format("YYYY-MM-DD 00:00:00.000"): ""
  const tableName = this.action ? this.action:""
  const action = this.activity ? this.activity: ""

  const url = `${this._url.audit.getauditdetails}?PageNumber=${pageNo}&PageSize=${pageSize}&employeeID=${search}`

  const body = {
  fromDate:  fromDate,
  toDate: toDate,
  auditType: action,
  tableName: tableName
  
  }
  console.log(body);

  this._http.post(url,body).subscribe(
    {
      next:(res:any)=>{

        this.LogAudits = res.data;
       
        let totalPage = res.totalPages;
        let itemsPerPage = res.pageSize;
        this.config.totalItems = totalPage * itemsPerPage;
        this._toast.success(res.responseMessage);
        console.log(res.responseMessage);
        // this.activity=""
        // this.FromDate=""
        // this.ToDate=""
        // this.action=""
      },
      error: (err: any) => {
        this._toast.error(err.error.data);
      },
    });
    
  }

  
 pageChanged(event){
  this.config.currentPage = event;
  this.Giveaudittrail();
}

searchItems(event: any) {
  this.config.search = event.target.value;
  this.searchTerm = event.target.value;
  this.Giveaudittrail();
}

public onactivityselect(event,value){
if(value == 'action'){
  this.action = event.target.value;
}
  if(value == 'activity'){
    this.activity = event.target.value;
  }

  console.log("event",event.target.value);
}

public filterdate(event,value){
  console.log("EVENT",event.target.value);
  if(value == 'fromdate'){
    this.FromDate = event.target.value;
  }
  if(value == 'todate'){
  this.ToDate = event.target.value;
}
  
}

public onSubmit(){
  this.Giveaudittrail();
}

  }


