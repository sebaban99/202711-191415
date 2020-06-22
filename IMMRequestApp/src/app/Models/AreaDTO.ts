import { TopicDTO } from './TopicDTO';

export class AreaDTO{
    id: string;
    name: string;
    topics: Array<TopicDTO>;
}