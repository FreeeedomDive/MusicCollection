import React from "react";
import {RootDto} from "../Dto/RootDto";
import "./RootsListElement.css"
import FilesApiClient from "../Api/FilesApiClient";

interface Props {
    root: RootDto
}

export default function RootsListElement({root}: Props): React.ReactElement {
    async function deleteRoot() {
        await FilesApiClient.deleteRoot(root.id);
        alert("Task was created")
    }

    return (
        <tr className="tableRow">
            <td className="tableElement">{root.id.toString()}</td>
            <td className="tableElement">{root.name}</td>
            <td className="tableElement">{root.path}</td>
            <td className="tableElement">
                <div className="deleteButton" onClick={deleteRoot}>Delete</div>
            </td>
        </tr>
    )
}