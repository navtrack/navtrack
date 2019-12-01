import React from "react";
import { Link } from "react-router-dom";

export default function AdminNavbar() {
    return (
        <nav className="navbar navbar navbar-expand-md navbar-dark bg-dark">
            <div className="container-fluid">
                <a className="h4 mb-0 text-white text-uppercase d-none d-lg-inline-block"
                    href="./index.html">Dashboard</a>
                <ul className="navbar-nav align-items-center d-none d-md-flex">
                    <li className="nav-item dropdown">
                        <a className="nav-link pr-0" href="#tmp" role="button" data-toggle="dropdown"
                            aria-haspopup="true"
                            aria-expanded="false">
                            <div className="media align-items-center">
                                <span className="avatar avatar-sm rounded-circle bg-white">
                                    <i className="fa fa-user text-gray-dark fa-lg" />
                                </span>
                                <div className="media-body ml-2 d-none d-lg-block">
                                    <span className="mb-0 text-sm  font-weight-bold">contact@navtrack.io</span>
                                </div>
                            </div>
                        </a>
                        <div className="dropdown-menu dropdown-menu-arrow dropdown-menu-right">
                            <div className=" dropdown-header noti-title">
                                <h6 className="text-overflow m-0">Welcome!</h6>
                            </div>
                            <a href="./examples/profile.html" className="dropdown-item">
                                <i className="ni ni-single-02"></i>
                                <span>My profile</span>
                            </a>
                            <a href="./examples/profile.html" className="dropdown-item">
                                <i className="ni ni-settings-gear-65"></i>
                                <span>Settings</span>
                            </a>
                            <a href="./examples/profile.html" className="dropdown-item">
                                <i className="ni ni-calendar-grid-58"></i>
                                <span>Activity</span>
                            </a>
                            <a href="./examples/profile.html" className="dropdown-item">
                                <i className="ni ni-support-16"></i>
                                <span>Support</span>
                            </a>
                            <div className="dropdown-divider"></div>
                            <Link to="/logout" className="dropdown-item">
                                <i className="fas fa-sign-out-alt"/><span>Logout</span>
                            </Link>
                        </div>
                    </li>
                </ul>
            </div>
        </nav>
    );
}