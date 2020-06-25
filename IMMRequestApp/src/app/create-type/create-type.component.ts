import { Component, OnInit } from '@angular/core';
import { TypeService } from '../type.service';
import { AreaDTO } from '../Models/AreaDTO';
import { TopicDTO } from '../Models/TopicDTO';
import { RangeDTO } from '../Models/RangeDTO';
import { AdditionalFieldDTO } from '../Models/AdditionalFieldDTO';
import * as $ from 'jquery';
import { TypeDTO } from '../Models/TypeDTO';


@Component({
  selector: 'app-create-type',
  templateUrl: './create-type.component.html',
  styleUrls: ['./create-type.component.css']
})

export class CreateTypeComponent implements OnInit {

  areas: Array<AreaDTO>;
  selectedAreaId: string;
  selectedArea: AreaDTO;
  topics: Array<TopicDTO>;
  selectedTopicId: string;

  typeName: string;

  afName: string;
  fieldType: number;
  hasRange: boolean;
  aFDTOs: Array<AdditionalFieldDTO>;
  
  constructor(private typeService: TypeService) { }

  ngOnInit(): void {
    this.getAreas();
    this.aFDTOs = new Array<AdditionalFieldDTO>();
  }

  getAreas(): void{
    this.typeService.getAreas()
    .subscribe(x => {this.areas = x}
    ,error => {alert(error)});
  }

  setTopicOfArea(){
    this.selectedArea = this.areas.find(a => a.id == this.selectedAreaId)
    this.topics = this.selectedArea.topics;
  }

  cleanModal(){
    this.afName = undefined;
    this.fieldType = undefined;
    this.hasRange = false;
  }

  createAdditionalField(){
    let newAdditionalField = new AdditionalFieldDTO();
    newAdditionalField.name = this.afName;
    newAdditionalField.fieldType = parseInt(this.fieldType + "");
    let range = new Array<RangeDTO>();
    console.log("esta llegando 1");
    console.log(this.hasRange);
    if(this.hasRange != undefined && this.hasRange == true){
      if(this.fieldType == 0){
        let minValue = new RangeDTO();
        let maxValue = new RangeDTO();
        minValue.value = $("#rangeDateMinValue").val() as string;
        maxValue.value = $("#rangeDateMaxValue").val() as string;
        range.push(minValue);
        range.push(maxValue);
      }
      else if(this.fieldType == 2){
        let minValue = new RangeDTO();
        let maxValue = new RangeDTO();
        minValue.value = $("#rangeNumberMinValue").val() as string;
        maxValue.value = $("#rangeNumberMaxValue").val() as string;
        range.push(minValue);
        range.push(maxValue);
      }
      else{
        let rawValues = $("#rangeTextValues").val() as string;
        let values = rawValues.split(";")
        console.log(values);
        values.forEach(function(val){
          if(val != ""){
            let rangeValue = new RangeDTO();
            rangeValue.value = val;
            range.push(rangeValue);
          }
        })
      }
    }
    console.log("esta llegando 2")
    newAdditionalField.rangeDTOs = range;
    this.aFDTOs.push(newAdditionalField);
    this.cleanModal();
    alert("Campo adicional agregado con éxito!")
  }

  createType(): void{
    let newType = new TypeDTO();
    console.log(this.typeName);
    newType.name = this.typeName;
    newType.topicId = this.selectedTopicId;
    newType.aFDTOs = this.aFDTOs;
    this.typeService.postType(newType).subscribe(res => {
      alert("El tipo se a registrado correctamente!");
      },error => {alert(error)});
  }
}
