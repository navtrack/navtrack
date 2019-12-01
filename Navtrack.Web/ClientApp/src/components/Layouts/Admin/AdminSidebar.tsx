import React from "react";
import { Link, useHistory } from "react-router-dom";

export default function AdminSidebar() {
    const history = useHistory();


    const navItemClassName = (url: string): string => {
        if (history.location.pathname.includes(url)) {
            return "nav-item active";
        }

        return "nav-item";
    }

    return (
        <nav className="navbar navbar-vertical fixed-left navbar-expand-md navbar-light bg-white" id="sidenav-main">
            <div className="container-fluid">

                <button className="navbar-toggler" type="button" data-toggle="collapse"
                    data-target="#sidenav-collapse-main" aria-controls="sidenav-main" aria-expanded="false"
                    aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon"></span>
                </button>

                <a className="navbar-brand pt-0" href="./index.html">
                    <img src="./assets/img/brand/blue.png" className="navbar-brand-img" alt="..." />
                </a>

                <ul className="nav align-items-center d-md-none">
                    <li className="nav-item dropdown">
                        <a className="nav-link nav-link-icon" href="#tmp" role="button" data-toggle="dropdown"
                            aria-haspopup="true" aria-expanded="false">
                            <i className="ni ni-bell-55"></i>
                        </a>
                        <div className="dropdown-menu dropdown-menu-arrow dropdown-menu-right"
                            aria-labelledby="navbar-default_dropdown_1">
                            <a className="dropdown-item" href="#tmp">Action</a>
                            <a className="dropdown-item" href="#tmp">Another action</a>
                            <div className="dropdown-divider"></div>
                            <a className="dropdown-item" href="#tmp">Something else here</a>
                        </div>
                    </li>
                    <li className="nav-item dropdown">
                        <a className="nav-link" href="#tmp" role="button" data-toggle="dropdown" aria-haspopup="true"
                            aria-expanded="false">
                            <div className="media align-items-center">
                                <span className="avatar avatar-sm rounded-circle">
                                    <img alt="placeholder" src="./assets/img/theme/team-1-800x800.jpg" />
                                </span>
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
                            <a href="#!" className="dropdown-item">
                                <i className="ni ni-user-run"></i>
                                <span>Logout</span>
                            </a>
                        </div>
                    </li>
                </ul>

                <div className="collapse navbar-collapse" id="sidenav-collapse-main">

                    <div className="navbar-collapse-header d-md-none">
                        <div className="row">
                            <div className="col-6 collapse-brand">
                                <a href="./index.html">
                                    <img alt="tmp" src="./assets/img/brand/blue.png" />
                                </a>
                            </div>
                            <div className="col-6 collapse-close">
                                <button type="button" className="navbar-toggler" data-toggle="collapse"
                                    data-target="#sidenav-collapse-main" aria-controls="sidenav-main"
                                    aria-expanded="false" aria-label="Toggle sidenav">
                                    <span></span>
                                    <span></span>
                                </button>
                            </div>
                        </div>
                    </div>

                    <form className="mt-4 mb-3 d-md-none">
                        <div className="input-group input-group-rounded input-group-merge">
                            <input type="search" className="form-control form-control-rounded form-control-prepended"
                                placeholder="Search" aria-label="Search" />
                            <div className="input-group-prepend">
                                <div className="input-group-text">
                                    <span className="fa fa-search"></span>
                                </div>
                            </div>
                        </div>
                    </form>

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

                    <hr className="my-3" />

                    <h6 className="navbar-heading text-muted">Documentation</h6>

                    <ul className="navbar-nav mb-md-3">
                        <li className="nav-item">
                            <a className="nav-link"
                                href="https://demos.creative-tim.com/argon-dashboard/docs/getting-started/overview.html">
                                <i className="ni ni-spaceship" /> Getting started
                            </a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link"
                                href="https://demos.creative-tim.com/argon-dashboard/docs/foundation/colors.html">
                                <i className="ni ni-palette" /> Foundation
                            </a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link"
                                href="https://demos.creative-tim.com/argon-dashboard/docs/components/alerts.html">
                                <i className="ni ni-ui-04" /> Components
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    );
}