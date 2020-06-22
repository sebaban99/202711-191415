import {AdditionalFieldDTO} from './AdditionalFieldDTO';
export class TypeDTO{
    id: string;
    name: string;
    topicId: string;
    isActive: boolean;
    aFDTOs: Array<AdditionalFieldDTO>;
}