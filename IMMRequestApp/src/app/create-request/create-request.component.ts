import { Component, OnInit, Input, ɵConsole, Output, EventEmitter } from '@angular/core';
import {RequestService} from '../request.service';
import {RequestDTO} from '../Models/RequestDTO';
import {AFValueDTO} from '../Models/AFValueDTO';
import {Router} from '@angular/router';
import { AreaDTO } from '../Models/AreaDTO';
import { TypeDTO } from '../Models/TypeDTO';
import { AdditionalFieldDTO } from '../Models/AdditionalFieldDTO';
import { TopicDTO } from '../Models/TopicDTO';
import * as $ from 'jquery';
import { AFValueItemDTO } from '../Models/AFValueItemDTO';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

@Component({
  selector: 'app-create-request',
  templateUrl: './create-request.component.html',
  styleUrls: ['./create-request.component.css']
})

export class CreateRequestComponent implements OnInit {

  @Input() displayedRequest: RequestDTO;
  @Input() isBeingEdited: boolean;
  @Output() modifiedRequestEmitter = new EventEmitter<RequestDTO>();

  id: string;
  requestNumber: number;
  details: string;
  name: string;
  email: string;
  phone: string;
  status: string;
  statusAsNum: number;
  description: string;
  creationDate: string;
  addFieldValuesDTOs: Array<AFValueDTO>;

  @Input() areas: Array<AreaDTO>;
  selectedAreaId: string;
  selectedArea: AreaDTO;
  
  topics: Array<TopicDTO>;
  selectedTopicId: string;
  selectedTopic: TopicDTO;
  
  types: Array<TypeDTO>;
  selectedTypeId: string;
  selectedType: TypeDTO;

  AddFields: Array<AdditionalFieldDTO>;

  constructor(private requestService: RequestService, private router: Router) { }

  ngOnInit(): void {
    if(this.displayedRequest != undefined){
      this.details = this.displayedRequest.details;
      this.selectedTypeId = this.displayedRequest.typeId
      this.name = this.displayedRequest.name;
      this.email = this.displayedRequest.email;
      this.phone = this.displayedRequest.phone;
      this.description = this.displayedRequest.description;
      this.addFieldValuesDTOs = this.displayedRequest.addFieldValuesDTOs;
      this.creationDate = this.displayedRequest.creationDate;
      console.log(this.creationDate);

      if(this.displayedRequest.status == 0){
        this.status = "Creada";
      }
      else if(this.displayedRequest.status == 1){
        this.status = "En revisión";
      }
      else if(this.displayedRequest.status == 2){
        this.status = "Aceptada";
      }
      else if(this.displayedRequest.status == 3){
        this.status = "Denegada";
      }
      else if(this.displayedRequest.status == 4){
        this.status = "Finalizada";
      }

      //$("#status-select").val(this.displayedRequest.status);

      this.selectedTypeId = this.displayedRequest.typeId;
      /*console.log("TypeId:" + this.selectedTypeId)
      if(this.areas != undefined){
        this.areas.forEach(function(area){
          console.log(area.name);
          area.topics.forEach(function(topic){
            topic.types.forEach(function(type){
              if(type.id == this.selectedTypeId){
                this.selectedArea = area;
                this.selectedTopic = topic;
                this.selectedType = type;
              }
            })
          })
        })
        
      }
      */
      
    }
    else{
      this.getAreas();
    }
  }

  setTopicOfArea(){
    this.selectedArea = this.areas.find(a => a.id == this.selectedAreaId)
    this.topics = this.selectedArea.topics;
    this.types = undefined;
    this.selectedType = undefined;
  }

  setTypesOfTopic(){
    this.selectedType = undefined;
    this.selectedTopic = this.topics.find(t => t.id == this.selectedTopicId);
    this.types = this.selectedTopic.types;
  }

  setAddFields(){
    this.selectedType = this.types.find(t => t.id == this.selectedTypeId);
    this.AddFields = this.selectedType.aFDTOs;
    console.log(this.selectedType);
    console.log(this.AddFields);

  }

  getAreas(): void{
    this.selectedType = undefined;
    this.selectedTopic = undefined;
    this.requestService.getAreas()
    .subscribe((x: Array<AreaDTO>) => {this.areas = x}
    ,error => {alert(error)});
  }

  updateRequest(){
    this.displayedRequest.status = parseInt(this.statusAsNum + "");
    this.displayedRequest.description = this.description;
    this.modifiedRequestEmitter.emit(this.displayedRequest);
  }

  createRequest() {

    let newRequestDTO = new RequestDTO();
    newRequestDTO.typeId = this.selectedType.id;
    newRequestDTO.name = this.name;
    newRequestDTO.email = this.email;
    newRequestDTO.phone = this.phone + "";
    newRequestDTO.details = this.details;
    console.log(newRequestDTO.details);
    
    this.selectedType.aFDTOs.forEach(function(addField){
      let afv = new AFValueDTO();
      afv.additionalFieldId = addField.id;
      if(addField.fieldType == 3){
        let fieldValue = $("input[name='radioValue']:checked").val() as string;
        let afvItem = new AFValueItemDTO();
        afvItem.value = fieldValue;
        afv.valuesItemDTOs.push(afvItem);
      }
      else{
        let rawFieldValues = $("#"+`${addField.id}`).val() as string;
        let values = rawFieldValues.split(";");
        values.forEach(function (value){
          if(value != ""){
            let afvItem = new AFValueItemDTO();
            afvItem.value = value;
            afv.valuesItemDTOs.push(afvItem);
          }
        })
      }
      newRequestDTO.addFieldValuesDTOs.push(afv);
    });
    

    this.requestService.CreateRequest(newRequestDTO).subscribe(res => {
      alert("Request created succesfully! Request number is: " + res);
      this.router.navigate['/home'];
    },error => {alert(error.error)});
    

  }

 

}
