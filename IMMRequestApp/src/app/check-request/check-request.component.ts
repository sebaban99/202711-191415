import { Component, OnInit } from '@angular/core';
import { RequestService } from '../request.service';
import { RequestDTO } from '../Models/RequestDTO';

@Component({
  selector: 'app-check-request',
  templateUrl: './check-request.component.html',
  styleUrls: ['./check-request.component.css']
})
export class CheckRequestComponent implements OnInit {

  requestNumber: number;
  selectedRequest: RequestDTO;
  
  constructor(private requestService: RequestService) { }

  ngOnInit(): void {
  }

  checkRequest(){
    this.requestService.GetByReqNumber(this.requestNumber).subscribe(
      (res: RequestDTO) => {this.selectedRequest = res}
      ,error => {alert("Error: Solicitud no encontrada, verifique el número")});
  }
}
