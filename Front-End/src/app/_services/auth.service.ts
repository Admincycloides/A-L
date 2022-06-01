import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private _router: Router) { }
  isLoggedIn() {
    const tokenExpiryTime = JSON.parse(localStorage.getItem('user')).tokenExpiryTime;
    const time = Math.floor(Date.now() / 1000);
    if(tokenExpiryTime < time) return true;
    else return false;
  }
  logout() {
    localStorage.removeItem("user");
    this._router.navigate(["/auth"]);
  }
}
