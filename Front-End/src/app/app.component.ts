import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './_services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit{
  user:any

  constructor(private _router: Router,private _authService:AuthService) { }

  ngOnInit(): void {
    if(!this._authService.isLoggedIn()){
      this._router.navigate(["/login"]);
    }
    // if(this._authService.isLoggedIn()) {
    //   //this._router.navigate(["/timesheet"]);
    // }else{
    //   this._router.navigate(["/login"]);
    // }
  }

}
