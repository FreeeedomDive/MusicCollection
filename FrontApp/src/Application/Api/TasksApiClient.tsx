import axios from "axios";
import {Guid} from "guid-typescript";
import TaskDto from "../Dto/TaskDto";

export default class TasksApiClient {
    static init = () => {
        return axios.create({
            baseURL: `adminApi/tasks`, timeout: 10000, headers: {
                Accept: "application/json"
            }
        });
    }

    static getTasks = async (): Promise<TaskDto[]> => {
        const result = await TasksApiClient.init().get<TaskDto[]>("");
        return result.data;
    }

    static removeFromQueue = async (taskId: Guid): Promise<void> => {
        await TasksApiClient.init().delete(`/${taskId}`);
    }
}
