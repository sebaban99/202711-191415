import { ImportationFieldDTO } from './ImportationFieldDTO';

export class ImporterDTO{
    importerName: string;
    requiredFields: Array<ImportationFieldDTO>;
}