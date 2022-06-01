import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UrlService {

  constructor() { }
  getRootURL(): String {
    const URL = environment.apiConfig.server;
    return URL;
  }
  getAPIURL(): String {
    const URL = environment.apiConfig.api;
    return URL;
  }
  // api       = environment.apiConfig.api;
  // serverApi = environment.apiConfig.server;



  // // Login
  // generateOTP = this.api + '/GenerateOTP';
  // submitOTP   = this.api + '/SubmitOTP';

  // Login
  public login = {
    getOTP        :'/GenerateOTP',
    submitOTP     :'/SubmitOTP'
  }
}
