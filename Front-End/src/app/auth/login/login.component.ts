import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  formSubmitted = false;
  loginForm!: FormGroup;
  errMessage:string;
  loginOtpForm:FormGroup
  get f() {
    return this.loginForm.controls;
  }
  get f1(){
    return this.loginOtpForm.controls;
  }
  constructor(private _fb: FormBuilder,private _router: Router,private toast: ToastrService) { }

  ngOnInit(): void {
    this.loginForm = this._fb.group({
      email: ['', [Validators.required,Validators.email]],
    });
    this.loginOtpForm = this._fb.group({
      otp: ['', Validators.required],
    });
  }
  onSubmitMail(){
    this.toast.success("User Successfully logged in")
    // if(this.loginForm.valid){
    //   const params = {
    //     emailAddress: this.loginForm.controls['email'].value,
    //   }
    //   this.authServce.get(url,params).subscribe((res)=>{
    //     if(res.responseCode == 200){
            //this.formSubmitted = true;
    //     }
    //   },
    //   (err: any) => {
    //     this.errMessage = err;
    //   }
    //   )
    // }
    //else{
      //this.toast.error("Please Enter Valid Email Address");
    //}
  }
  login(){
    // if(this.loginForm.valid){
    //   const params = {
    //     otp: this.loginOtpForm.controls['otp'].value,
    //   }
    //   this.authServce.get(url,params).subscribe((res)=>{
    //     if(res.responseCode == 200){
              //var data = response.data;
              //localStorage.setItem('user',JSON.stringify(data));
              //this.toast.success("User Successfully logged in")
              //this._router.navigate(['/home']);
    //     }
    //   },
    //   (err: any) => {
    //     this.errMessage = err;
    //   }
    //   )
    // }
    //else{
      //this.toast.error("Please Enter Valid OTP");
    //}

  }

}
