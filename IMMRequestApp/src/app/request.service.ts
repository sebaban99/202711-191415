import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient,HttpHeaders, HttpErrorResponse} from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { RequestDTO } from './Models/RequestDTO';
import { SessionService } from './session.service';
import {AreaDTO} from './Models/AreaDTO';

@Injectable({
  providedIn: 'root'
})
export class RequestService {

  private URL: string = environment.APIURL + "/Request";
  private checkReqURL: string = environment.APIURL + "/Request/Check";
  private areas: string = environment.APIURL + "/Area";



  constructor(private sessionService: SessionService,private httpService: HttpClient) { }

  CreateRequest(request: RequestDTO){
    return this.httpService.post(this.URL, request)
    .pipe(catchError((error: HttpErrorResponse) => throwError(error.error)));
  }

  getAreas(): Observable<Array<AreaDTO>> {
    const options = this.sessionService.getOptions();
    const lista = this.httpService.get<Array<AreaDTO>>(this.areas, options)
    .pipe(
      catchError((error: HttpErrorResponse) => throwError(error.error || 'server error'))
      );
    return lista;
  }
  

  PutConcrete(request: RequestDTO, reqId: string) {
    const options = this.sessionService.getOptions();
    return this.httpService.put(this.URL + '/' + reqId, request, options)
    .pipe(catchError((error: HttpErrorResponse) => throwError(error.error)));
  }

  GetByReqNumber(reqNumber: number): Observable<RequestDTO> {
    return this.httpService.get<RequestDTO>(this.checkReqURL + '/' + reqNumber)
    .pipe(catchError((error: HttpErrorResponse) => throwError(error.error)));
  }
}
