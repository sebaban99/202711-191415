import {AFValueDTO} from './AFValueDTO';
export class RequestDTO {
    Id: string;
    RequestNumber: number;
    Details: string;
    TypeId: string;
    Name: string;
    Email: string;
    Phone: string;
    Status: number;
    Description: string;
    AddFieldValuesDTOs: Array<AFValueDTO>;
}