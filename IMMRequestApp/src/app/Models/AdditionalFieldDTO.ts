import {RangeDTO} from './RangeDTO';
export class AdditionalFieldDTO{
    Id: string;
    Name: string;
    FieldType: number;
    Type: string;
    RangeDTOs: Array<RangeDTO>;
}