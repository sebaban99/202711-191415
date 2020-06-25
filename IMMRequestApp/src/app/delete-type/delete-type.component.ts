import { Component, OnInit } from '@angular/core';
import {TypeDTO} from '../Models/TypeDTO';
import { TypeService } from '../type.service';
import { TopicDTO } from '../Models/TopicDTO';
import { AreaDTO } from '../Models/AreaDTO';

@Component({
  selector: 'app-delete-type',
  templateUrl: './delete-type.component.html',
  styleUrls: ['./delete-type.component.css']
})
export class DeleteTypeComponent implements OnInit {

  areas: Array<AreaDTO>;
  selectedAreaId: string;
  selectedArea: AreaDTO;
  topics: Array<TopicDTO>;
  selectedTopicId: string;
  selectedTopic: TopicDTO;
  types: Array<TypeDTO>;
  selectedTypeId: string;

  constructor(private typeService: TypeService) { }

  
  ngOnInit(): void {
    this.getAreas();
  }

  deleteType(){
    this.typeService.deleteType(this.selectedTypeId)
    .subscribe(res =>{
      alert("Type deleated successfully!");
      this.getAreas();
      }
    ,error => {alert(error)});
  }

  setTopicOfArea(){
    this.selectedArea = this.areas.find(a => a.id == this.selectedAreaId)
    this.topics = this.selectedArea.topics;
    this.types = undefined;
  }

  setTypesOfTopic(){
    this.selectedTopic = this.topics.find(t => t.id == this.selectedTopicId);
    this.types = this.selectedTopic.types;
  }

  getAreas(): void{
    this.typeService.getAreas()
    .subscribe(x => {this.areas = x}
    ,error => {alert(error)});
  }

}
