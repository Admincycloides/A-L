import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private _router: Router) { }
  isLoggedIn() {
    // //const tokenExpiryTime = JSON.parse(localStorage.getItem('user')).tokenExpiryTime;
    // const tokenExpiryTime = JSON.parse(localStorage.getItem('token')).tokenExpiryDate ? JSON.parse(localStorage.getItem('token')).tokenExpiryDate:'NA';
    // //const time = Math.floor(Date.now() / 1000);
    // const time = new Date().toLocaleString();
    // if(tokenExpiryTime !='NA' && tokenExpiryTime > time) return true;
    // else{
    //   //localStorage.removeItem("user");
    //   localStorage.removeItem("token");
    //   return false;
    // }
    return true;
  }
  logout() {
    //localStorage.removeItem("user");
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    this._router.navigate(["/login"]);
  }
}
