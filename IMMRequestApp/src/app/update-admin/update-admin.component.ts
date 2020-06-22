import { Component, OnInit} from '@angular/core';
import {AdminDTO} from '../Models/AdminDTO';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-update-admin',
  templateUrl: './update-admin.component.html',
  styleUrls: ['./update-admin.component.css']
})
export class UpdateAdminComponent implements OnInit {

  admins: Array<AdminDTO>;
  selectedAdminId: string;
  selectedAdmin: AdminDTO;

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.getAdmins();
  }

  getAdmins(){
    this.adminService.getAdmins()
    .subscribe(x => {this.admins = x}
    ,error => {alert(error)});
  }

  setSelected(selectedVendor) {
    this.selectedAdmin = selectedVendor;
    let newTodo = Object.assign(new AdminDTO(), selectedVendor); 
    console.log(newTodo);
    console.log(newTodo.Name);
    console.log(newTodo["id"]);
  }

  getAdmin(){
    console.log(this.selectedAdminId);
    this.adminService.getById(this.selectedAdminId)
    .subscribe((res: AdminDTO) => {
      this.selectedAdmin = res,
      console.log(res)}
      ,error => {alert(error)});
  }

  checkAdmin(){
    console.log("SelectedAdmin name : " + this.selectedAdminId);
  }

  updateAdmin(event){
    console.log("llegue aca");
    this.adminService.putAdmin(event ,this.selectedAdmin.id)
    .subscribe(res =>{
      alert("Admin updated successfully!");
      this.getAdmins();
      }
    ,error => {alert(error)});
  }
}
