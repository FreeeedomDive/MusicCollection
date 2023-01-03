import React from "react";
import "./AdminAppElement.css"

interface Props {
    name: string,
    onClick: () => void
}

export default function AdminAppElement(props: Props): React.ReactElement {
    return (
        <div className="adminAppElement">
            <div onClick={props.onClick}>{props.name}</div>
        </div>
    );
}