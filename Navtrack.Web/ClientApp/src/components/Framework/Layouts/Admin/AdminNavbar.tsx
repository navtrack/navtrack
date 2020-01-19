import React, { useContext } from "react";
import { useHistory } from "react-router-dom";
import { AccountApi } from "services/Api/AccountApi";
import AppContext from "services/AppContext";

export default function AdminNavbar() {
    const { appContext, setAppContext } = useContext(AppContext);
    const history = useHistory();

    const handleLogout = () => {
        AccountApi.logout().then(x => {
            if (x.ok) {
                setAppContext({ ...appContext, user: { username: "", authenticated: false } });
                history.push("/");
            }
        })
    }

    return (
        <nav className="navbar navbar-expand-md navbar-dark bg-dark">
            <div className="flex-fill text-white">NAVTRACK</div>
                <ul className="navbar-nav align-items-center d-none d-md-flex">
                    <li className="nav-item dropdown">
                        <a className="nav-link p-0" href="#tmp" role="button" data-toggle="dropdown"
                            aria-haspopup="true"
                            aria-expanded="false">
                            <div className="media align-items-center">
                                    <i className="fa fa-user" />
                                <div className="media-body ml-2 d-none d-lg-block">
                                    <span className="mb-0 text-sm  font-weight-bold">{appContext.user.username}</span>
                                </div>
                            </div>
                        </a>
                        <div className="dropdown-menu dropdown-menu-arrow dropdown-menu-right">
                            <div className=" dropdown-header noti-title">
                                <h6 className="text-overflow m-0">Welcome!</h6>
                            </div>
                            {/* <a href="./examples/profile.html" className="dropdown-item">
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
                            <div className="dropdown-divider"></div> */}
                            <div className="btn-link dropdown-item font-weight-normal" onClick={handleLogout}>
                                <i className="fas fa-sign-out-alt" /> Logout
                            </div>
                        </div>
                    </li>
                </ul>
        </nav>
    );
}