import axios from "axios";
import {RootDto} from "../../Dto/RootDto";

export default class FilesApiClient {
    static init = () => {
        return axios.create({
            baseURL: "https://localhost:7039", timeout: 10000, headers: {
                Accept: "application/json"
            }
        });
    }

    static createRoot = async (name: string, path: string): Promise<void> => {
        const result = await FilesApiClient.init().post("/files/roots/create", {
            name: name,
            path: path
        });
        return result.data;
    }

    static getAllRoots = async (): Promise<RootDto[]> => {
        const result = await FilesApiClient.init().get<RootDto[]>(`/files/roots`);
        return result.data;
    }
}