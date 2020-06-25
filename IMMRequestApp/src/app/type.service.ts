import { Injectable } from '@angular/core';
import { TypeDTO } from './Models/TypeDTO';
import { SessionService } from './session.service';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { AreaDTO } from './Models/AreaDTO';

@Injectable({
  providedIn: 'root'
})
export class TypeService {

  private URL: string = environment.APIURL + "/Type";
  private areaURL: string = environment.APIURL + "/Area";


  constructor(private sessionService: SessionService,private httpService: HttpClient) { }

  postType(type: TypeDTO) {
    const options = this.sessionService.getOptions();
    return this.httpService.post(this.URL, type, options)
    .pipe(catchError((error: HttpErrorResponse) => throwError(error.error)));
  }

  deleteType(typeID: string) {
    const options = this.sessionService.getOptions();
    return this.httpService.delete(this.URL + '/' + typeID, options)
    .pipe(catchError((error: HttpErrorResponse) => throwError(error.error)));
  }

  getAreas(): Observable<Array<AreaDTO>> {
    const options = this.sessionService.getOptions();
    const lista = this.httpService.get<Array<AreaDTO>>(this.areaURL)
    .pipe(
      catchError((error: HttpErrorResponse) => throwError(error.error || 'server error'))
      );
    return lista;
  }
}
