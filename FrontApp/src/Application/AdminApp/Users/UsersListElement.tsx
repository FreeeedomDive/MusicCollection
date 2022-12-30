import UserDto from "../Dto/UserDto";
import UsersApiClient from "../Api/UsersApiClient";
import "./UsersListElement.css"
import React from "react";

interface Props {
    user: UserDto,
    updateList: () => Promise<void>
}

export default function UsersListElement(props: Props): React.ReactElement {
    async function banUser(){
        await UsersApiClient.ban(props.user.id);
        await props.updateList();
    }

    return (
        <tr className="tableRow">
            <td className="tableElement">{props.user.id.toString()}</td>
            <td className="tableElement">{props.user.login}</td>
            <td className="tableElement"><div className="banButton" onClick={banUser}>Ban</div></td>
        </tr>
    )
}