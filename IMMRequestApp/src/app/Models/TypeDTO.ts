import {AdditionalFieldDTO} from './AdditionalFieldDTO';
export class TypeDTO{
    Id: string;
    Name: string;
    TopicId: string;
    IsActive: boolean;
    aFDTOs: Array<AdditionalFieldDTO>;
}