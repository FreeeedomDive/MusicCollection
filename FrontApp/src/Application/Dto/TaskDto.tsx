import {Guid} from "guid-typescript";

export default interface TaskDto {
    id: Guid
    type: string
    state: string
    progress: number
}