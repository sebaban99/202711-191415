import { Component, OnInit } from '@angular/core';
import {AdminDTO} from '../Models/AdminDTO';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-delete-admin',
  templateUrl: './delete-admin.component.html',
  styleUrls: ['./delete-admin.component.css']
})
export class DeleteAdminComponent implements OnInit {
  
  admins: Array<AdminDTO>;
  selectedAdminId: string;

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.getAdmins();
  }

  getAdmins(){
    this.adminService.getAdmins()
    .subscribe(x => {this.admins = x}
    ,error => {alert(error)});
  }

  deleteAdmin(){
    console.log(this.selectedAdminId);
    this.adminService.deleteAdmin(this.selectedAdminId)
    .subscribe(res =>{
      alert("Admin deleated successfully!");
      this.getAdmins();
      }
    ,error => {alert(error)});
  }
}
