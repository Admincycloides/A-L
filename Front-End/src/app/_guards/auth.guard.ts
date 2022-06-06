import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { AuthService } from "app/_services/auth.service";

@Injectable({ providedIn: "root" })
export default class AuthGuard {
  constructor(private _authService: AuthService, private _router: Router) {}
  canActivate(): any {
    if (this._authService.isLoggedIn()) return true;
    // else navigate to login
    this._router.navigate(["/login"]);
  }
}