import React, { useContext } from "react";
import { Link } from "react-router-dom";
import AppContext from "services/AppContext";

export default function AdminSidebar() {
    const { appContext } = useContext(AppContext);

    return (
        <nav className="p-3 bg-white w-48 text-sm shadow z-10">
            <h5 className="mb-2 text-gray-500 uppercase tracking-wide font-semibold text-xs">Assets</h5>
            {appContext.assets ? appContext.assets.length > 0 ?
                <ul>
                    {appContext.assets.map(x =>
                        <li key={x.id} className="mb-2">
                            <Link className="text-gray-600 hover:text-gray-900" to={`/live/${x.id}`}>
                                <i className="fa fa-circle mr-1 w-4 text-center text-green-600 text-xs" /> {x.name}
                            </Link>
                        </li>)}
                </ul>
                :
                <div>No assets.</div>
                :
                <div>Loading assets.</div>
            }
            <hr className="my-3" />
            <h5 className="mb-2 text-gray-500 uppercase tracking-wide font-semibold text-xs">Management</h5>
            <ul>
                <li className="mb-2">
                    <Link className="text-gray-600 hover:text-gray-900" to="/assets">
                        <i className="fa fa-car-alt mr-1 w-4 text-center" /> Assets
                    </Link>
                </li>
                <li className="mb-2">
                    <Link className="text-gray-600 hover:text-gray-900" to="/devices">
                        <i className="fas fa-hdd mr-1 w-4" /> Devices
                    </Link>
                </li>
            </ul>
        </nav>
    );
}