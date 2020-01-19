import React, { useContext } from "react";
import { Link, useHistory } from "react-router-dom";
import AppContext from "services/AppContext";

export default function AdminSidebar() {
    const history = useHistory();
    const { appContext } = useContext(AppContext);

    const navItemClassName = (url: string): string => {
        return history.location.pathname.includes(url) ? "nav-item active" : "nav-item";
    }

    return (
        <nav className="p-3 navbar-vertical sidebar navbar-light bg-white">
                <h6 className="navbar-heading text-muted">Assets</h6>

                {appContext.assets ? appContext.assets.length > 0 ?
                    <ul className="navbar-nav">
                        {appContext.assets.map(x =>
                            <li key={x.id} className={navItemClassName("/live")}>
                                <Link className="nav-link" to={`/live/${x.id}`}>
                                    <i className="fas fa-circle fa-xs text-green" /> {x.name}
                                </Link>
                            </li>)}
                    </ul>
                    :
                    <h5>No assets.</h5>
                    :
                    <h5>Loading assets.</h5>
                }
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
        </nav>
    );
}