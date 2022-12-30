import React from "react";
import {Route, Routes} from "react-router-dom";
import RootsComponent from "./Roots/RootsComponent";

import './AdminAppRouter.css';
import UsersComponent from "./Users/UsersComponent";
import AdminAppElement from "./AdminAppElement";
import TasksComponent from "./Tasks/TasksComponent";

export default function AdminAppRouter(): React.ReactElement {
    return (
        <div className="wrapper">
            <div className="adminPanelElements">
                <AdminAppElement name={"Users"} redirectLink={"/admin/users"}/>
                <AdminAppElement name={"Roots"} redirectLink={"/admin/roots"}/>
                <AdminAppElement name={"Tasks"} redirectLink={"/admin/tasks"}/>
            </div>
            <div className="adminPanelViewer">
                <Routes>
                    <Route path={`/users`} element={<UsersComponent/>}/>
                    <Route path={`/roots`} element={<RootsComponent/>}/>
                    <Route path={`/tasks`} element={<TasksComponent/>}/>
                    <Route path={`/`} element={<div></div>}/>
                </Routes>
            </div>
        </div>
    );
}