import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ImporterDTO } from './Models/ImporterDTO';
import { ImportInfoDTO } from './Models/ImportInfoDTO';

@Injectable({
  providedIn: 'root'
})
export class ImportationService {

  private URL: string = environment.APIURL + "/Importation";
  
  constructor(private httpService: HttpClient) { }

  getImportationMethods(): Observable<Array<ImporterDTO>> {
    const lista = this.httpService.get<Array<ImporterDTO>>(this.URL)
    .pipe(
      catchError((error: HttpErrorResponse) => throwError(error.error || 'server error'))
      );
    return lista;
  }

  postElements(importInfo: ImportInfoDTO) {
    return this.httpService.post(this.URL, importInfo)
    .pipe(catchError((error: HttpErrorResponse) => throwError(error.error)));
  }
}
