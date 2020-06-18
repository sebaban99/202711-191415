import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient,HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import {LoginDTO} from './Models/LoginDTO';


@Injectable({
  providedIn: 'root'
})
export class SessionService {

  private URL: string = environment.APIURL + "/Login";

  constructor(private http: HttpClient) { }

  Login(login: LoginDTO) {
    return this.http.post(this.URL, login)
      .pipe(map(session => {
        if (session) {
          sessionStorage.setItem('currentUser', session["id"]);
          return session;
        }
      }, catchError((error: HttpErrorResponse) => throwError(error.error))));
  }

  getOptions(){
    const token= sessionStorage.getItem('currentUser');
    let headers: HttpHeaders = new HttpHeaders();
    headers = headers.append('Authorization',token);
    headers = headers.append('Content-Type','application/json');
    return {headers};
  }

  logout() {
    localStorage.removeItem('currentUser');
  }
}