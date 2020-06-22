import { Component, OnInit } from '@angular/core';
import { ImportationService } from '../importation.service';
import { ImporterDTO } from '../Models/ImporterDTO';
import { ImportInfoDTO } from '../Models/ImportInfoDTO';
import * as $ from 'jquery';

@Component({
  selector: 'app-importation',
  templateUrl: './importation.component.html',
  styleUrls: ['./importation.component.css']
})
export class ImportationComponent implements OnInit {

  importationMethods: Array<ImporterDTO>;
  selectedImporter: ImporterDTO;
  selectedImpName: string;
  
  constructor(private importationService: ImportationService) { }

  setImporter(){
    this.selectedImporter = this.importationMethods.find(i => i.importerName == this.selectedImpName);
  }
  
  ngOnInit(): void {
    this.getImportationMethods();
  }

  getImportationMethods(): void{
    this.importationService.getImportationMethods()
    .subscribe(x => {this.importationMethods = x}
    ,error => {alert(error)});
  }

  import(){
    this.selectedImporter.requiredFields.forEach(function (field){
      field.fieldValue = $("#" + `${field.nameOfField}`).val() as string;
      console.log(field.fieldValue);
    });
    /*let impInfo = new ImportInfoDTO();
    impInfo.importationMethod = this.selectedImpName;
    impInfo.requiredFields = this.selectedImporter.requiredFields;
    this.importationService.postElements(impInfo).subscribe(res => {
    alert("Elements imported successfully!");
    },error => {alert(error)});
    */
  }

}
