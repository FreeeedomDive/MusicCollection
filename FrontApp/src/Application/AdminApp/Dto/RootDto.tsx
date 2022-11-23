import {Guid} from "guid-typescript";

export interface RootDto {
    id: Guid;
    name: string;
    path: string;
}