import {AFValueItemDTO} from './AFValueItemDTO';
export class AFValueDTO{
    id: string;
    requestId: string;
    additionalFieldId: string;
    valuesItemDTOs: Array<AFValueItemDTO>;
}
