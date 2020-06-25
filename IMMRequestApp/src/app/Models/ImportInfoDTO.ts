import { ImportationFieldDTO } from './ImportationFieldDTO';

export class ImportInfoDTO{
    importationMethod: string;
    requiredFields: Array<ImportationFieldDTO>;
}