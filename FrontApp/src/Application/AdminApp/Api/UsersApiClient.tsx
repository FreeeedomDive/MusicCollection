import axios from "axios";
import UserDto from "../Dto/UserDto";
import {Guid} from "guid-typescript";

export default class UsersApiClient {
    static init = () => {
        return axios.create({
            baseURL: "https://localhost:7039/users", timeout: 10000, headers: {
                Accept: "application/json"
            }
        });
    }

    static getUsers = async (): Promise<UserDto[]> => {
        const result = await UsersApiClient.init().get<UserDto[]>("");
        return result.data;
    }

    static ban = async (userId: Guid): Promise<void> => {
        await UsersApiClient.init().delete(`/${userId}/ban`);
    }
}