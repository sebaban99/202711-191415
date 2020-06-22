import {AFValueDTO} from './AFValueDTO';
export class RequestDTO {
    id: string;
    requestNumber: number;
    details: string;
    typeId: string;
    name: string;
    email: string;
    phone: string;
    status: number;
    description: string;
    addFieldValuesDTOs: Array<AFValueDTO>;
}