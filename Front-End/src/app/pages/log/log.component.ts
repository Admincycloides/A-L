import { Component, OnInit } from '@angular/core';
import { HttpService } from 'app/_services/http.service';
import { UrlService } from 'app/_services/url.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-log',
  templateUrl: './log.component.html',
  styleUrls: ['./log.component.scss']
})
export class LogComponent implements OnInit {

  public LogAudits = [];

  constructor(private _http: HttpService,private _url: UrlService,) { }

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
