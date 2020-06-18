import {AFValueItemDTO} from './AFValueItemDTO';
export class AFValueDTO{
    Id: string;
    RequestId: string;
    AdditionalFieldId: string;
    ValuesItemDTOs: Array<AFValueItemDTO>;
}
