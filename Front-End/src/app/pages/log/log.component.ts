import { Component, OnInit } from '@angular/core';
import { HttpService } from 'app/_services/http.service';
import { UrlService } from 'app/_services/url.service';
import { NgbModal,NgbModalConfig } from '@ng-bootstrap/ng-bootstrap';
import { IDropdownSettings } from "ng-multiselect-dropdown";


@Component({
  selector: 'app-log',
  templateUrl: './log.component.html',
  styleUrls: ['./log.component.scss'],
  providers: [NgbModalConfig, NgbModal]
})
export class LogComponent implements OnInit {

  public LogAudits = [];
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
  }


