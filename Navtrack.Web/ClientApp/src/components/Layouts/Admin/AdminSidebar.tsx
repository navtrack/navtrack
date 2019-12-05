import React from "react";
import { Link, useHistory } from "react-router-dom";

export default function AdminSidebar() {
    const history = useHistory();

    const navItemClassName = (url: string): string => {
        return history.location.pathname.includes(url) ? "nav-item active" : "nav-item";
    }

    return (
        <nav className="navbar navbar-vertical fixed-left navbar-expand-md navbar-light bg-white" id="sidenav-main">
            <div className="container-fluid">
                <a className="navbar-brand pt-0" href="./index.html">
                    <img src="./assets/img/brand/blue.png" className="navbar-brand-img" alt="..." />
                </a>
                <div className="collapse navbar-collapse">
                    <ul className="navbar-nav">
                        <li className={navItemClassName("/live")}>
                            <Link className="nav-link" to="/live">
                                <i className="fas fa-map-marked-alt" /> Live Tracking
                            </Link>
                        </li>
                    </ul>
                    <hr className="my-3" />
                    <h6 className="navbar-heading text-muted">Management</h6>
                    <ul className="navbar-nav">
                        <li className={navItemClassName("/assets")}>
                            <Link className="nav-link" to="/assets">
                                <i className="fas fa-map-marker-alt" /> Assets
                            </Link>
                        </li>
                        <li className={navItemClassName("/devices")}>
                            <Link className="nav-link" to="/devices">
                                <i className="fas fa-hdd" /> Devices
                            </Link>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    );
}