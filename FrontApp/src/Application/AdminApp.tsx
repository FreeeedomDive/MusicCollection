import React, {useState} from "react";
import RootsComponent from "./Roots/RootsComponent";

import './AdminApp.css';
import UsersComponent from "./Users/UsersComponent";
import AdminAppElement from "./AdminAppElement";
import TasksComponent from "./Tasks/TasksComponent";

export default function AdminApp(): React.ReactElement {
    const [showUsers, setShowUsers] = useState(false)
    const [showRoots, setShowRoots] = useState(false)
    const [showTasks, setShowTasks] = useState(false)

    function setVisibleComponent(setComponent: () => void) {
        setShowUsers(false)
        setShowRoots(false)
        setShowTasks(false)

        setComponent()
    }

    return (
        <div className="wrapper">
            <div className="adminPanelElements">
                <AdminAppElement name={"Users"} onClick={() => setVisibleComponent(() => setShowUsers(true))}/>
                <AdminAppElement name={"Roots"} onClick={() => setVisibleComponent(() => setShowRoots(true))}/>
                <AdminAppElement name={"Tasks"} onClick={() => setVisibleComponent(() => setShowTasks(true))}/>
            </div>
            <div className="adminPanelViewer">
                {showUsers && <UsersComponent/>}
                {showRoots && <RootsComponent/>}
                {showTasks && <TasksComponent/>}
            </div>
        </div>
    );
}