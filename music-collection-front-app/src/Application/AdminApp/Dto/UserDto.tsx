import {Guid} from "guid-typescript";

export default interface UserDto {
    id: Guid
    login: string
}