import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpService } from 'app/_services/http.service';
import { UrlService } from 'app/_services/url.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  formSubmitted = false;
  loginForm!: FormGroup;
  errMessage: string ;
  invalidOtp: string;
  loginOtpForm:FormGroup
  get f() {
    return this.loginForm.controls;
  }
  get f1(){
    return this.loginOtpForm.controls;
  }
  constructor(private _fb: FormBuilder,private _router: Router,private toast: ToastrService,
    private _url: UrlService,
    private _http: HttpService
    ) { }

  ngOnInit(): void {
    this.loginForm = this._fb.group({
      email: ['', [Validators.required,Validators.email]],
    });
    this.loginOtpForm = this._fb.group({
      otp: ['', Validators.required],
    });
  }
  onSubmitMail(){
   if(this.loginForm.valid){
      let emailAddress = this.loginForm.controls['email'].value;
      const url = `${this._url.user.getOTP}?mailto=${emailAddress}`
      this._http.post(url,{}).subscribe(
        {
          next(res) {
            this.formSubmitted = true;
          },
          error(msg) {
            this.errMessage = msg;
          }
        })
    }
    else{
      this.toast.error("Please Enter Valid Email Address");
    }
  }
  login(){
    if(this.loginForm.valid){
       let  otpValue = this.loginOtpForm.controls['otp'].value;
       let username = this.loginForm.controls['email'].value;
      //let otpValue ='123456789';
      //let username ='jishnup@tangentia.com'
      this._http.get(`${this._url.user.submitOTP}/${otpValue}/${username}`).subscribe(
        {
          next(res) {
              //var data = res.body.data;
              //localStorage.setItem('token',JSON.stringify(data));
              this.toast.success("User Successfully logged in");
              this._router.navigate(['/timesheet']);
          },
          error(msg) {
            this.errMessage = msg;
          }
        }
      )
    }
    else{
      this.toast.error("Please Enter Valid OTP");
    }

  }
  onResentOtp(){
      let  emailAddress = this.loginForm.controls['email'].value
      this._http.get(`${this._url.user.getOTP}?mailto=${emailAddress}`).subscribe(
        {
          next(res) {
            this.formSubmitted = true;
          },
          error(msg) {
            this.errMessage = msg;
          }
        }
      )
  }

}
