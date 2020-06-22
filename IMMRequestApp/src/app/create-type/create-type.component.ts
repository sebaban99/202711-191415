import { Component, OnInit } from '@angular/core';
import { TypeService } from '../type.service';
import { AreaDTO } from '../Models/AreaDTO';
import { TopicDTO } from '../Models/TopicDTO';
import { RangeDTO } from '../Models/RangeDTO';
import { AdditionalFieldDTO } from '../Models/AdditionalFieldDTO';


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

  name: string;
  fieldType: number;
  range: Array<RangeDTO>;
  aFDTOs: Array<AdditionalFieldDTO>;
  
  constructor(private typeService: TypeService) { }

  ngOnInit(): void {
    this.getAreas();
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

  createType(): void{

  }
}
