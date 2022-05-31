import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit{
  user:any

  constructor(private _router: Router) { }

  ngOnInit(): void {
    this.user = JSON.parse(localStorage.getItem('user'))
    const time = Math.floor(Date.now() / 1000);
    if(this.user.tokenExpiryTime < time){
      this._router.navigate(["/home"]);
    }
    else{
      this._router.navigate(["/login"]);
    }
  }

}
