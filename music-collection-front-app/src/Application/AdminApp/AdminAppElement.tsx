import React from "react";
import {useNavigate} from "react-router-dom";
import "./AdminAppElement.css"

interface Props {
    name: string,
    redirectLink: string
}

export default function AdminAppElement(props: Props): JSX.Element {
    const navigate = useNavigate();
    return (
        <div className="adminAppElement">
            <div onClick={() => navigate(props.redirectLink)}>{props.name}</div>
        </div>
    );
}