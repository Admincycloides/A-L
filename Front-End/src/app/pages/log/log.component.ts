import { Component, OnInit } from '@angular/core';
import { HttpService } from 'app/_services/http.service';
import { UrlService } from 'app/_services/url.service';
import { NgbModal,NgbModalConfig } from '@ng-bootstrap/ng-bootstrap';
import { IDropdownSettings } from "ng-multiselect-dropdown";
import { Tooltip } from 'chart.js';


@Component({
  selector: 'app-log',
  templateUrl: './log.component.html',
  styleUrls: ['./log.component.scss'],
  providers: [NgbModalConfig, NgbModal]
})
export class LogComponent implements OnInit {

  public LogAudits = [];
  tooltipName:any;
  tooltipDescription:any;
  dropdownEmployeeSettings: IDropdownSettings = {};

  constructor(private _http: HttpService,private _url: UrlService,config: NgbModalConfig, private modalService: NgbModal) { 
    config.backdrop = 'static';
    config.keyboard = false;
  }
  open(content) {
    this.modalService.open(content);
  }
  ngOnInit(): void {

    this.getAuditLogs();
  }


  private getAuditLogs(){
    const url = `${this._url.audit.auditlog}`
    this._http.get(url).subscribe({
      next:(res:any)=>{
          this.LogAudits = res.data;
          console.log("hello",this.LogAudits);
      }
        })
    }

    public getName(value:any){

      let newString;
      if(value.length > 100 ){
        console.log("goo",value);
      newString = value.substring(0,80)+'....';
      // this.tooltipDescription
    }
  return newString
}
  }


