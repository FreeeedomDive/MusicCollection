import React, {useEffect, useState} from "react";
import UsersApiClient from "../Api/Users/UsersApiClient";
import UsersListElement from "./UsersListElement";
import UserDto from "../Dto/UserDto";
import "./UsersComponent.css"

export default function UsersComponent(): JSX.Element {
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
            <h2>Users</h2>
            <table>
                <tr>
                    <th>Id</th>
                    <th>Username</th>
                    <th>Ban</th>
                </tr>
                {
                    usersList.map(x => <UsersListElement user={x} updateList={updateList}/>)
                }
            </table>
        </div>
    );
}