import React, {useEffect, useState} from "react";
import TaskDto from "../Dto/TaskDto";
import TasksApiClient from "../Api/TasksApiClient";
import TasksListElement from "./TasksListElement";
import "./TasksComponent.css"

export default function TasksComponent(): React.ReactElement {
    const [tasksList, setTasks] = useState<TaskDto[]>([])

    async function updateList(): Promise<void> {
        const tasks = await TasksApiClient.getTasks()
        setTasks(tasks)
    }

    useEffect(() => {
        updateList().catch(console.error)
        const interval = setInterval(() => updateList().catch(console.error), 1000);
        return () => {
            clearInterval(interval);
        };
    }, []);

    return (
        <div>
            <div className="tasksHeader"><b>Tasks</b></div>
            {tasksList.length === 0
                ? <div className="tasksHeader">No running or scheduled tasks yet...</div>
                : (
                    <table className="tasksTable">
                        <tr>
                            <th>Id</th>
                            <th>Type</th>
                            <th>State</th>
                            <th>Progress</th>
                            <th></th>
                        </tr>
                        {
                            tasksList.map(x => <TasksListElement task={x}/>)
                        }
                    </table>
                )
            }
        </div>
    );
}