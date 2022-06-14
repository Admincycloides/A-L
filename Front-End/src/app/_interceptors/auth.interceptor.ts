import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from "app/_services/auth.service";

@Injectable({ providedIn: "root" })
export class AuthHeaderInterceptor implements HttpInterceptor {
    constructor(private _router: Router, private _authService: AuthService) { }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        //let modifiedReq: any;
        if (this._authService.isLoggedIn()) {
            // Getting token
            const token = JSON.parse(localStorage.getItem('user')).token;
            // Creating new request
            req = req.clone({
                headers: req.headers.set('authorization', `Bearer ${token}`), // returns modified headers
              });
        } 
        return next.handle(req);
        // else {

        //     let error = new HttpErrorResponse({ error: "Token Expired", status: 401 });
		// 	// TODO : throw an error alert!
        //     this._router.navigate(['/login'])
        //     throw error;
        // }
    }
}
