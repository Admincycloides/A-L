import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private _router: Router) { }
  isLoggedIn() {
    //const tokenExpiryTime = JSON.parse(localStorage.getItem('user')).tokenExpiryTime;
    const tokenExpiryTime = new Date(JSON.parse(localStorage.getItem('user'))?.tokenExpiryDate);
    //const time = Math.floor(Date.now() / 1000);
    
    const time = new Date();
    //console.log("tokenExpiryTime > time",tokenExpiryTime > time)
    if(tokenExpiryTime > time){
      return true;
    }
    else{
      //localStorage.removeItem("user");
      localStorage.removeItem('user');
      return false;
    }
    //return true;
  }
  logout() {
    //localStorage.removeItem("user");
    localStorage.removeItem("user");
    this._router.navigate(["/login"]);
  }
}
