import { Component, OnInit, Input, Output } from '@angular/core';
import {AdminService} from '../admin.service';
import {AdminDTO} from '../Models/AdminDTO';
import { EventEmitter } from '@angular/core';

@Component({
  selector: 'app-create-admin',
  templateUrl: './create-admin.component.html',
  styleUrls: ['./create-admin.component.css']
})
export class CreateAdminComponent implements OnInit {

  @Input() adminDisplayed: AdminDTO;
  @Output() modifiedAdminEmitter = new EventEmitter<AdminDTO>();
  name: string;
  email: string;
  password: string;

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    if(this.adminDisplayed != null){
      console.log(this.adminDisplayed.name);
      this.name = this.adminDisplayed.name;
      this.email = this.adminDisplayed.email;
      this.password = this.adminDisplayed.password;
    }
  }

  onSubmitUpdate() {
    this.adminDisplayed.name = this.name;
    this.adminDisplayed.email = this.email;
    this.adminDisplayed.password = this.password;
    this.modifiedAdminEmitter.emit(this.adminDisplayed);
    console.log("llegue al emit");

  }

  onSubmitCreate() {
    let adminDTO = new AdminDTO();
    adminDTO.password = this.password;
    adminDTO.email = this.email;
    adminDTO.name = this.name;
    console.log("llego aca por lo menos");
    this.adminService.postAdmin(adminDTO).subscribe(res => {
    alert("Admin has been registered successfully!");
    },error => {alert(error)});
  }
}
