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
    creationDate: string;
    addFieldValuesDTOs: Array<AFValueDTO>;

    constructor() {
        this.addFieldValuesDTOs = new Array<AFValueDTO>();
    }
}