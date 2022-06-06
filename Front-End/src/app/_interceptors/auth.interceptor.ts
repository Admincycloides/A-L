// import { Injectable } from "@angular/core";
// import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
// import { Observable } from 'rxjs';
// import { AuthService } from 'src/app/_services/auth.service';
// import { Router } from '@angular/router';

// @Injectable()
// export class AuthHeaderInterceptor implements HttpInterceptor {
//     constructor(private _router: Router, private _authService: AuthService) { }
//     intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
//         if (this._authService.isLoggedIn()) {
//             // Getting token
//             const token = localStorage.getItem('jwt_token');
//             // Creating new request
//             const modifiedReq = req.clone({
//                 headers: req.headers.set('Authorization', `${token}`), // returns modified headers
//               });
//             return next.handle(modifiedReq);
//         } else {
//             let error = new HttpErrorResponse({ error: "Token Expired", status: 401 });
// 			// TODO : throw an error alert!
//             this._router.navigate(['/auth'])
//             throw error;
//         }
//     }
// }
