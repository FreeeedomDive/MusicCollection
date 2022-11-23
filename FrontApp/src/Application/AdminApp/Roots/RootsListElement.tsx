import React from "react";
import {RootDto} from "../Dto/RootDto";
import "./RootsListElement.css"

interface Props {
    root: RootDto
}

export default function RootsListElement({root}: Props) : React.ReactElement {
    return (
        <tr className="tableRow">
            <td className="tableElement">{root.id.toString()}</td>
            <td className="tableElement">{root.name}</td>
            <td className="tableElement">{root.path}</td>
        </tr>
    )
}