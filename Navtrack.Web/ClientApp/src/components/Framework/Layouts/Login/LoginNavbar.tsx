import React from "react";

export default function LoginNavbar() {
    return (
        <nav className="navbar navbar-horizontal navbar-dark">
            <a className="navbar-brand" href="/">
                <h1 className="text-white">Navtrack</h1>
            </a>
            <ul className="navbar-nav ml-auto">
                <li className="nav-item">
                    <a className="nav-link nav-link-icon" href="https://www.navtrack.io">
                        <i className="fas fa-home" />
                        <span className="nav-link-inner--text">Home</span>
                    </a>
                </li>
            </ul>
        </nav>
    );
}