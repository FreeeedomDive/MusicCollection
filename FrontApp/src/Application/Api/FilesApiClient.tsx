import axios from "axios";
import {RootDto} from "../Dto/RootDto";
import {Guid} from "guid-typescript";

export default class FilesApiClient {
    static init = () => {
        return axios.create({
            baseURL: `adminApi/files`, timeout: 10000, headers: {
                Accept: "application/json"
            }
        });
    }

    static createRoot = async (name: string, path: string): Promise<void> => {
        const result = await FilesApiClient.init().post("/roots/create", {
            name: name,
            path: path
        });
        return result.data;
    }

    static deleteRoot = async (id: Guid): Promise<void> => {
        const result = await FilesApiClient.init().delete(`/roots/${id}`);
        return result.data;
    }

    static getAllRoots = async (): Promise<RootDto[]> => {
        const result = await FilesApiClient.init().get<RootDto[]>("/roots");
        return result.data;
    }
}