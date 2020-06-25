import { Component, OnInit } from '@angular/core';
import { ImportationService } from '../importation.service';
import { ImporterDTO } from '../Models/ImporterDTO';
import { ImportInfoDTO } from '../Models/ImportInfoDTO';
import * as $ from 'jquery';
import { ImportationFieldHelper } from '../Models/ImportationFieldHelper';

@Component({
  selector: 'app-importation',
  templateUrl: './importation.component.html',
  styleUrls: ['./importation.component.css']
})
export class ImportationComponent implements OnInit {

  importationMethods: Array<ImporterDTO>;
  selectedImporter: ImporterDTO;
  selectedImpName: string;
  requiredFields: Array<ImportationFieldHelper>;
  
  constructor(private importationService: ImportationService) { }

  setImporter(){
    this.selectedImporter = this.importationMethods.find(i => i.importerName == this.selectedImpName);
    let elemenTindex = 0;
    let requiredHelpers = new Array<ImportationFieldHelper>();
    this.selectedImporter.requiredFields.forEach(function(field){
      let impFieldHelper = new ImportationFieldHelper();
      impFieldHelper.index = "reqVal-" + elemenTindex;
      impFieldHelper.field = field;
      requiredHelpers.push(impFieldHelper);
      elemenTindex++;
    })
    this.requiredFields = requiredHelpers;
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
    let elemenTindex = 0;
    this.selectedImporter.requiredFields.forEach(function (field){
      field.fieldValue = $("#reqVal-" + `${elemenTindex}`).val() as string;
      elemenTindex++;
    });
    let impInfo = new ImportInfoDTO();
    impInfo.importationMethod = this.selectedImpName;
    impInfo.requiredFields = this.selectedImporter.requiredFields;
    this.importationService.postElements(impInfo).subscribe(res => {
    alert("Elements imported successfully!");
    },error => {alert(error)});
  }

}
