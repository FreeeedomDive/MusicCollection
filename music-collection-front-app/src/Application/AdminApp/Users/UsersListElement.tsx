import UserDto from "../Dto/UserDto";
import UsersApiClient from "../Api/Users/UsersApiClient";
import "./UsersListElement.css"

interface Props {
    user: UserDto,
    updateList: () => Promise<void>
}

export default function UsersListElement(props: Props): JSX.Element {
    async function banUser(){
        await UsersApiClient.ban(props.user.id);
        await props.updateList();
    }

    return (
        <tr>
            <td className="tableElement">{props.user.id.toString()}</td>
            <td className="tableElement">{props.user.login}</td>
            <td className="tableElement"><div className="banButton" onClick={banUser}>БАН НАХУЙ</div></td>
        </tr>
    )
}