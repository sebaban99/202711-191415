import {RangeDTO} from './RangeDTO';
export class AdditionalFieldDTO{
    id: string;
    name: string;
    fieldType: number;
    type: string;
    rangeDTOs: Array<RangeDTO>;

    constructor() {}
}