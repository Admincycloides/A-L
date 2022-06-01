import { Injectable } from "@angular/core";
import {
  HttpClient,
  HttpHeaders,
  HttpParams,
  HttpResponse,
} from "@angular/common/http";
import { Observable } from "rxjs";
import { UrlService } from "../_services/url.service";

@Injectable()
export class HttpService {
  constructor(private _http: HttpClient, private _urlService: UrlService) {}
  // get(
  //   url: string,
  //   options: {
  //     headers?: HttpHeaders | { [header: string]: string | string[] };
  //     observe: "response";
  //     params?: HttpParams | { [param: string]: string | string[] };
  //     reportProgress?: boolean;
  //     responseType?: "json";
  //     withCredentials?: boolean;
  //   }
  // ): Observable<HttpResponse<Object>>;

  // GET
  get(
    url: String,
    query?: String,
    options?: { headers?: HttpHeaders }
  ): Observable<Object> {
    let urlString: string;

    query
      ? (urlString = `${this._urlService.getAPIURL()}/${url}?${query}`)
      : (urlString = `${this._urlService.getAPIURL()}/${url}`);

    const promise$ = this._http.get(urlString, options);

    return promise$;
  }

  // DELETE
  delete(
    url: String,
    query?: String,
    options?: { headers?: HttpHeaders }
  ): Observable<Object> {
    let urlString: string;

    query
      ? (urlString = `${this._urlService.getAPIURL()}/${url}?${query}`)
      : (urlString = `${this._urlService.getAPIURL()}/${url}`);

    const promise$ = this._http.delete(urlString, options);

    return promise$;
  }

  // POST
  post(
    url: String,
    body: any,
    options?: { headers?: HttpHeaders }
  ): Observable<Object> {
    let urlString = `${this._urlService.getAPIURL()}/${url}`;
    const promise$ = this._http.post(urlString, body, options);

    return promise$;
  }

  // PATCH
  patch(
    url: String,
    body: any,
    options?: { headers?: HttpHeaders }
  ): Observable<Object> {
    let urlString = `${this._urlService.getAPIURL()}/${url}`;
    const promise$ = this._http.patch(urlString, body, options);

    return promise$;
  }
}