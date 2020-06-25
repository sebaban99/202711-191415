import { Component, OnInit } from '@angular/core';
import { RequestService } from '../request.service';
import { RequestDTO } from '../Models/RequestDTO';

@Component({
  selector: 'app-update-request',
  templateUrl: './update-request.component.html',
  styleUrls: ['./update-request.component.css']
})
export class UpdateRequestComponent implements OnInit {

  requests: Array<RequestDTO>;
  selectedRequestId: string;
  selectedRequest: RequestDTO;

  constructor(private requestService: RequestService) { }

  ngOnInit(): void {
    this.getRequests();
  }

  getRequests(){
    this.requestService.getRequests()
    .subscribe(x => {this.requests = x}
    ,error => {alert(error)});
  }

  updateRequest(event){
    this.requestService.PutRequest(event ,this.selectedRequestId)
    .subscribe(res =>{
      alert("Request updated successfully!");
      this.getRequests();
      }
    ,error => {alert(error)});
  }

  setRequest(){
    this.selectedRequest = this.requests.find(r => r.id == this.selectedRequestId);
  }
}
