import axios from "axios";
import {RootDto} from "../Dto/RootDto";
import ApiConstants from "./ApiConstants";

export default class FilesApiClient {
    static init = () => {
        return axios.create({
            baseURL: `${ApiConstants.AdminApiBaseRoute}/files`, timeout: 10000, headers: {
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

    static getAllRoots = async (): Promise<RootDto[]> => {
        const result = await FilesApiClient.init().get<RootDto[]>(`/roots`);
        return result.data;
    }
}