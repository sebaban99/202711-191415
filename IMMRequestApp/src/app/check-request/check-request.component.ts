import { Component, OnInit } from '@angular/core';
import { RequestService } from '../request.service';
import { RequestDTO } from '../Models/RequestDTO';
import { AreaDTO } from '../Models/AreaDTO';

@Component({
  selector: 'app-check-request',
  templateUrl: './check-request.component.html',
  styleUrls: ['./check-request.component.css']
})
export class CheckRequestComponent implements OnInit {

  requestNumber: number;
  selectedRequest: RequestDTO;
  areas: Array<AreaDTO>;
  
  constructor(private requestService: RequestService) { }

  ngOnInit(): void {
    this.getAreas();
  }

  getAreas(): void{
    this.requestService.getAreas()
    .subscribe(x => {this.areas = x}
    ,error => {alert(error)});
  }

  checkRequest(){
    this.requestService.GetByReqNumber(this.requestNumber).subscribe(
      (res: RequestDTO) => {this.selectedRequest = res}
      ,error => {alert("Error: Solicitud no encontrada, verifique el número")});
  }
}
