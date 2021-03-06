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

  CreateRequest(request: RequestDTO): Observable<number>{
    return this.httpService.post<number>(this.URL, request)
    .pipe(map(reqNumber => {
      if (reqNumber != null) {
        return reqNumber;
      }
    }, catchError((error: HttpErrorResponse) => throwError(error.error))));
  }

  getAreas(): Observable<Array<AreaDTO>> {
    const lista = this.httpService.get<Array<AreaDTO>>(this.areas)
    .pipe(
      catchError((error: HttpErrorResponse) => throwError(error.error || 'server error'))
      );
    return lista;
  }
  

  PutRequest(request: RequestDTO, reqId: string) {
    const options = this.sessionService.getOptions();
    return this.httpService.put(this.URL + '/' + reqId, request, options)
    .pipe(catchError((error: HttpErrorResponse) => throwError(error.error)));
  }

  GetByReqNumber(reqNumber: number): Observable<RequestDTO> {
    return this.httpService.get<RequestDTO>(this.checkReqURL + '/' + reqNumber)
    .pipe(catchError((error: HttpErrorResponse) => throwError(error.error)));
  }

  getRequests(): Observable<Array<RequestDTO>> {
    const options = this.sessionService.getOptions();
    const lista = this.httpService.get<Array<RequestDTO>>(this.URL, options)
    .pipe(
      catchError((error: HttpErrorResponse) => throwError(error.error))
      );
    return lista;
  }
}
