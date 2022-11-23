import React from "react";
import {
    BrowserRouter as Router,
    Routes,
    Route
} from "react-router-dom";
import AdminAppRouter from "./AdminApp/AdminAppRouter";

export default function AppRouter() {
    return (
        <Router>
            <Routes>
                <Route path="/admin/*" element={<AdminAppRouter />} />
                <Route path="/" element={<Home />} />
            </Routes>
        </Router>
    );
}

function Home() {
    return <h2>Home</h2>;
}