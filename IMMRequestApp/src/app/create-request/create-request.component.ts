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
  @Input() IsBeingDisplayed: boolean;
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
  Areas: Array<AreaDTO>

  constructor(private requestService: RequestService, private router: Router) { }

  ngOnInit(): void {
    if(this.IsBeingDisplayed == true){
      this.Id = this.displayedRequest.Id;
      this.RequestNumber = this.displayedRequest.RequestNumber;
      this.Details = this.displayedRequest.Details;
      this.TypeId = this.displayedRequest.TypeId
      this.Name = this.displayedRequest.Name;
      this.Email = this.displayedRequest.Email;
      this.Phone = this.displayedRequest.Phone;
      this.Status = this.displayedRequest.Status;
      this.Description = this.displayedRequest.Description;
      this.AddFieldValuesDTOs = this.displayedRequest.AddFieldValuesDTOs;
    }
    else{
      this.getAreas();
    }
    
  }

  getAreas(): void{
    //this.Areas = this.requestService.getAreas();
  }

  onSubmit() {

    let newRequestDTO = new RequestDTO();
    newRequestDTO.Id = this.Id;
    newRequestDTO.RequestNumber = this.RequestNumber;
    newRequestDTO.Details = this.Details;
    newRequestDTO.TypeId = this.TypeId;
    newRequestDTO.Name = this.Name;
    newRequestDTO.Email = this.Email;
    newRequestDTO.Phone = this.Phone;
    newRequestDTO.Status = this.Status;
    newRequestDTO.Description = this.Description;
    newRequestDTO.AddFieldValuesDTOs = this.AddFieldValuesDTOs;

    this.requestService.CreateRequest(newRequestDTO).subscribe(res => {
      alert("Request created succesfully! Request number is: ");
      this.router.navigate['/home'];
    },error => {alert(error.error)});

  }

}
