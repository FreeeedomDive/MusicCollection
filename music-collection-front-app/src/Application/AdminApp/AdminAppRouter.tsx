import React from "react";
import {Route, Routes} from "react-router-dom";
import RootsComponent from "./Roots/RootsComponent";

import './AdminAppRouter.css';
import UsersComponent from "./Users/UsersComponent";
import AdminAppElement from "./AdminAppElement";

export default function AdminAppRouter(): JSX.Element {
    return (
        <div className="wrapper">
            <div className="adminPanelElements">
                <AdminAppElement name={"Users"} redirectLink={"/admin/users"}/>
                <AdminAppElement name={"Roots"} redirectLink={"/admin/roots"}/>
            </div>
            <div className="adminPanelViewer">
                <Routes>
                    <Route path={`/users`} element={<UsersComponent/>}/>
                    <Route path={`/roots`} element={<RootsComponent/>}/>
                    <Route path={`/`} element={<div></div>}/>
                </Routes>
            </div>
        </div>
    );
}