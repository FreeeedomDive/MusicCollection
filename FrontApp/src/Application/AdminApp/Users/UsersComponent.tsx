import React, {useEffect, useState} from "react";
import UsersApiClient from "../Api/UsersApiClient";
import UsersListElement from "./UsersListElement";
import UserDto from "../Dto/UserDto";
import "./UsersComponent.css"

export default function UsersComponent(): React.ReactElement {
    const [usersList, setUsers] = useState<UserDto[]>([])

    async function updateList(): Promise<void> {
        const users = await UsersApiClient.getUsers()
        setUsers(users)
    }

    useEffect(() => {
        updateList().catch(console.error)
    }, []);

    return (
        <div>
            <div className="usersHeader"><b>Users</b></div>
            <table className="usersTable">
                <tr>
                    <th>Id</th>
                    <th>Username</th>
                    <th></th>
                </tr>
                {
                    usersList.map(x => <UsersListElement user={x} updateList={updateList}/>)
                }
            </table>
        </div>
    );
}