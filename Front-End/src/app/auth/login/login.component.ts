import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  formSubmitted = false;
  loginForm!: FormGroup;
  errMessage:string;
  get f() {
    return this.loginForm.controls;
  }

  constructor(private _fb: FormBuilder,private _router: Router) { }

  ngOnInit(): void {
    this.loginForm = this._fb.group({
      email: ['', Validators.required],
    });
  }
  login(event:any){
    this.formSubmitted = true;
    // if(this.loginForm.valid){
    //   const params = {
    //     email: this.loginForm.controls['email'].value,
    //   }
    //   this.authServce.get(url,params).subscribe((res)=>{
    //     if(res.responseCode == 200){
              //this._router.navigate(['']);
    //     }
    //   },
    //   (err: any) => {
    //     this.errMessage = err;
    //   }
    //   )
    // }
  }

}
