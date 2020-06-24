import { Component, OnInit } from '@angular/core';
import { AdminService } from '../admin.service';
import * as $ from 'jquery';
import { ReportAData } from '../Models/ReportAData';
import { ReportTypeAElement } from '../Models/ReportTypeAElement';
import { ReportBData } from '../Models/ReportBData';
import { ReportTypeBElement } from '../Models/ReportTypeBElement';


@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent implements OnInit {

  selectedReportType: string;
  reportA: Array<ReportTypeAElement>;
  reportB: Array<ReportTypeBElement>;


  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
  }

  generateReport(){
    if(this.selectedReportType == 'A'){
      let reportAData = new ReportAData();
      reportAData.Email = $("#email").val() as string;
      console.log(reportAData.Email);
      reportAData.From = $("#from").val() as string;
      console.log(reportAData.From);
      reportAData.Until = $("#until").val() as string;
      console.log(reportAData.Until);
      if(reportAData.Email == undefined ||
        reportAData.Until == undefined ||
        reportAData.From == undefined){
          alert("Por favor complete todos los campos")
        }
        else{
          this.adminService.generateReportA(reportAData).subscribe(
            x => {this.reportA = x}
            ,error => {alert(error)});
        }
    }
    else{
      let reportBData = new ReportBData();
      reportBData.from = $("#from").val() as string;
      reportBData.until = $("#until").val() as string;
      if(reportBData.until == undefined ||
        reportBData.from == undefined){
          alert("Por favor complete todos los campos")
        }
        else{
          this.adminService.generateReportB(reportBData).subscribe(
            x => {this.reportB = x}
            ,error => {alert(error)});
        }
    }
  }

}
