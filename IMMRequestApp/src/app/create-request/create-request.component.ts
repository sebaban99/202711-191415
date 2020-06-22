import { Component, OnInit, Input } from '@angular/core';
import {RequestService} from '../request.service';
import {RequestDTO} from '../Models/RequestDTO';
import {AFValueDTO} from '../Models/AFValueDTO';
import {Router} from '@angular/router';
import { AreaDTO } from '../Models/AreaDTO';

@Component({
  selector: 'app-create-request',
  templateUrl: './create-request.component.html',
  styleUrls: ['./create-request.component.css']
})

export class CreateRequestComponent implements OnInit {

  @Input() displayedRequest: RequestDTO;
  Id: string;
  RequestNumber: number;
  Details: string;
  TypeId: string;
  Name: string;
  Email: string;
  Phone: string;
  Status: number;
  Description: string;
  AddFieldValuesDTOs: Array<AFValueDTO>;
  areas: Array<AreaDTO>

  constructor(private requestService: RequestService, private router: Router) { }

  ngOnInit(): void {
    if(this.displayedRequest != null){
      this.Id = this.displayedRequest.id;
      this.RequestNumber = this.displayedRequest.requestNumber;
      this.Details = this.displayedRequest.details;
      this.TypeId = this.displayedRequest.typeId
      this.Name = this.displayedRequest.name;
      this.Email = this.displayedRequest.email;
      this.Phone = this.displayedRequest.phone;
      this.Status = this.displayedRequest.status;
      this.Description = this.displayedRequest.description;
      this.AddFieldValuesDTOs = this.displayedRequest.addFieldValuesDTOs;
    }
    else{
      this.getAreas();
    }
    
  }

  getAreas(): void{
    this.requestService.getAreas()
    .subscribe(x => {this.areas = x}
    ,error => {alert(error)});
  }

  onSubmit() {

    let newRequestDTO = new RequestDTO();
    newRequestDTO.id = this.Id;
    newRequestDTO.requestNumber = this.RequestNumber;
    newRequestDTO.details = this.Details;
    newRequestDTO.typeId = this.TypeId;
    newRequestDTO.name = this.Name;
    newRequestDTO.email = this.Email;
    newRequestDTO.phone = this.Phone;
    newRequestDTO.status = this.Status;
    newRequestDTO.description = this.Description;
    newRequestDTO.addFieldValuesDTOs = this.AddFieldValuesDTOs;

    this.requestService.CreateRequest(newRequestDTO).subscribe(res => {
      alert("Request created succesfully! Request number is: ");
      this.router.navigate['/home'];
    },error => {alert(error.error)});

  }

}
