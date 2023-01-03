import "./TasksListElement.css"
import React from "react";
import TasksApiClient from "../Api/TasksApiClient";
import TaskDto from "../Dto/TaskDto";

interface Props {
    task: TaskDto
}

export default function TasksListElement(props: Props): React.ReactElement {
    async function banUser() {
        await TasksApiClient.removeFromQueue(props.task.id);
    }

    return (
        <tr className="tableRow">
            <td className="tableElement">{props.task.id.toString()}</td>
            <td className="tableElement">{props.task.type}</td>
            <td className="tableElement">{props.task.state}</td>
            <td className="tableElement">{props.task.progress}</td>
            {
                (props.task.state === "Pending" || props.task.state === "Done" || props.task.state === "Fatal")
                && <td className="tableElement">
                    <div className="deleteTask" onClick={banUser}>Delete</div>
                </td>
            }
        </tr>
    )
}