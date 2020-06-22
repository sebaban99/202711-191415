import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { environment } from '../environments/environment';
import { map, catchError } from 'rxjs/operators';
import { HttpClient,HttpHeaders, HttpErrorResponse} from '@angular/common/http';
import { SessionService } from './session.service';
import {AdminDTO} from './Models/AdminDTO';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  private URL: string = environment.APIURL + "/Admin";

  constructor(private sessionService: SessionService,private httpService: HttpClient) { }

  getById(userID: string): Observable<AdminDTO> {
    const options = this.sessionService.getOptions();
    return this.httpService.get<AdminDTO>(this.URL + '/' + userID, options)
    .pipe(catchError((error: HttpErrorResponse) => throwError(error.error)));
  }

  getAdmins(): Observable<Array<AdminDTO>> {
    const options = this.sessionService.getOptions();
    const lista = this.httpService.get<Array<AdminDTO>>(this.URL, options)
    .pipe(
      catchError((error: HttpErrorResponse) => throwError(error.error))
      );
    return lista;
  }

  postAdmin(admin: AdminDTO) {
    const options = this.sessionService.getOptions();
    return this.httpService.post(this.URL, admin, options)
    .pipe(catchError((error: HttpErrorResponse) => throwError(error.error)));
  }

  putAdmin(admin: AdminDTO, adminID: string) {
    const options = this.sessionService.getOptions();
    return this.httpService.put(this.URL + '/' + adminID, admin, options)
    .pipe(catchError((error: HttpErrorResponse) => throwError(error.error)));
  }

  deleteAdmin(adminID: string) {
    const options = this.sessionService.getOptions();
    return this.httpService.delete(this.URL + '/' + adminID, options)
    .pipe(catchError((error: HttpErrorResponse) => throwError(error.error)));
  }
  
}
