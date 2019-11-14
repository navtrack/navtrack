import AdminSidebar from "./AdminSidebar";
import AdminFooter from "./AdminFooter";
import AdminNavbar from "./AdminNavbar";

export default function AdminLayout() {
    return (
        <>
            <AdminSidebar/>
            <div className="main-content">
                <!-- Navbar -->
                <AdminNavbar/>
                <!-- End Navbar -->
                <!-- Header -->
                <div className="header bg-gradient-primary pb-8 pt-5 pt-md-8">
                    <div className="container-fluid">
                        <div className="header-body">
                            <!-- Card stats -->
                            <div className="row">
                                <div className="col-xl-3 col-lg-6">
                                    <div className="card card-stats mb-4 mb-xl-0">
                                        <div className="card-body">
                                            <div className="row">
                                                <div className="col">
                                                    <h5 className="card-title text-uppercase text-muted mb-0">Traffic</h5>
                                                    <span className="h2 font-weight-bold mb-0">350,897</span>
                                                </div>
                                                <div className="col-auto">
                                                    <div
                                                        className="icon icon-shape bg-danger text-white rounded-circle shadow">
                                                        <i className="fas fa-chart-bar"></i>
                                                    </div>
                                                </div>
                                            </div>
                                            <p className="mt-3 mb-0 text-muted text-sm">
                                        <span className="text-success mr-2"><i
                                            className="fa fa-arrow-up"></i> 3.48%</span>
                                                <span className="text-nowrap">Since last month</span>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                                <div className="col-xl-3 col-lg-6">
                                    <div className="card card-stats mb-4 mb-xl-0">
                                        <div className="card-body">
                                            <div className="row">
                                                <div className="col">
                                                    <h5 className="card-title text-uppercase text-muted mb-0">New
                                                        users</h5>
                                                    <span className="h2 font-weight-bold mb-0">2,356</span>
                                                </div>
                                                <div className="col-auto">
                                                    <div
                                                        className="icon icon-shape bg-warning text-white rounded-circle shadow">
                                                        <i className="fas fa-chart-pie"></i>
                                                    </div>
                                                </div>
                                            </div>
                                            <p className="mt-3 mb-0 text-muted text-sm">
                                        <span className="text-danger mr-2"><i
                                            className="fas fa-arrow-down"></i> 3.48%</span>
                                                <span className="text-nowrap">Since last week</span>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                                <div className="col-xl-3 col-lg-6">
                                    <div className="card card-stats mb-4 mb-xl-0">
                                        <div className="card-body">
                                            <div className="row">
                                                <div className="col">
                                                    <h5 className="card-title text-uppercase text-muted mb-0">Sales</h5>
                                                    <span className="h2 font-weight-bold mb-0">924</span>
                                                </div>
                                                <div className="col-auto">
                                                    <div
                                                        className="icon icon-shape bg-yellow text-white rounded-circle shadow">
                                                        <i className="fas fa-users"></i>
                                                    </div>
                                                </div>
                                            </div>
                                            <p className="mt-3 mb-0 text-muted text-sm">
                                                <span className="text-warning mr-2"><i
                                                    className="fas fa-arrow-down"></i> 1.10%</span>
                                                <span className="text-nowrap">Since yesterday</span>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                                <div className="col-xl-3 col-lg-6">
                                    <div className="card card-stats mb-4 mb-xl-0">
                                        <div className="card-body">
                                            <div className="row">
                                                <div className="col">
                                                    <h5 className="card-title text-uppercase text-muted mb-0">Performance</h5>
                                                    <span className="h2 font-weight-bold mb-0">49,65%</span>
                                                </div>
                                                <div className="col-auto">
                                                    <div
                                                        className="icon icon-shape bg-info text-white rounded-circle shadow">
                                                        <i className="fas fa-percent"></i>
                                                    </div>
                                                </div>
                                            </div>
                                            <p className="mt-3 mb-0 text-muted text-sm">
                                        <span className="text-success mr-2"><i
                                            className="fas fa-arrow-up"></i> 12%</span>
                                                <span className="text-nowrap">Since last month</span>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="container-fluid mt--7">
                    <div className="row">
                        <div className="col-xl-8 mb-5 mb-xl-0">
                            <div className="card bg-gradient-default shadow">
                                <div className="card-header bg-transparent">
                                    <div className="row align-items-center">
                                        <div className="col">
                                            <h6 className="text-uppercase text-light ls-1 mb-1">Overview</h6>
                                            <h2 className="text-white mb-0">Sales value</h2>
                                        </div>
                                        <div className="col">
                                            <ul className="nav nav-pills justify-content-end">
                                                <li className="nav-item mr-2 mr-md-0" data-toggle="chart"
                                                    data-target="#chart-sales"
                                                    data-update='{"data":{"datasets":[{"data":[0, 20, 10, 30, 15, 40, 20, 60, 60]}]}}'
                                                    data-prefix="$" data-suffix="k">
                                                    <a href="#" className="nav-link py-2 px-3 active" data-toggle="tab">
                                                        <span className="d-none d-md-block">Month</span>
                                                        <span className="d-md-none">M</span>
                                                    </a>
                                                </li>
                                                <li className="nav-item" data-toggle="chart" data-target="#chart-sales"
                                                    data-update='{"data":{"datasets":[{"data":[0, 20, 5, 25, 10, 30, 15, 40, 40]}]}}'
                                                    data-prefix="$" data-suffix="k">
                                                    <a href="#" className="nav-link py-2 px-3" data-toggle="tab">
                                                        <span className="d-none d-md-block">Week</span>
                                                        <span className="d-md-none">W</span>
                                                    </a>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                                <div className="card-body">
                                    <!-- Chart -->
                                    <div className="chart">
                                        <!-- Chart wrapper -->
                                        <canvas id="chart-sales" className="chart-canvas"></canvas>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="col-xl-4">
                            <div className="card shadow">
                                <div className="card-header bg-transparent">
                                    <div className="row align-items-center">
                                        <div className="col">
                                            <h6 className="text-uppercase text-muted ls-1 mb-1">Performance</h6>
                                            <h2 className="mb-0">Total orders</h2>
                                        </div>
                                    </div>
                                </div>
                                <div className="card-body">
                                    <!-- Chart -->
                                    <div className="chart">
                                        <canvas id="chart-orders" className="chart-canvas"></canvas>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="row mt-5">
                        <div className="col-xl-8 mb-5 mb-xl-0">
                            <div className="card shadow">
                                <div className="card-header border-0">
                                    <div className="row align-items-center">
                                        <div className="col">
                                            <h3 className="mb-0">Page visits</h3>
                                        </div>
                                        <div className="col text-right">
                                            <a href="#!" className="btn btn-sm btn-primary">See all</a>
                                        </div>
                                    </div>
                                </div>
                                <div className="table-responsive">
                                    <!-- Projects table -->
                                    <table className="table align-items-center table-flush">
                                        <thead className="thead-light">
                                        <tr>
                                            <th scope="col">Page name</th>
                                            <th scope="col">Visitors</th>
                                            <th scope="col">Unique users</th>
                                            <th scope="col">Bounce rate</th>
                                        </tr>
                                        </thead>
                                        <tbody>
                                        <tr>
                                            <th scope="row">
                                                /argon/
                                            </th>
                                            <td>
                                                4,569
                                            </td>
                                            <td>
                                                340
                                            </td>
                                            <td>
                                                <i className="fas fa-arrow-up text-success mr-3"></i> 46,53%
                                            </td>
                                        </tr>
                                        <tr>
                                            <th scope="row">
                                                /argon/index.html
                                            </th>
                                            <td>
                                                3,985
                                            </td>
                                            <td>
                                                319
                                            </td>
                                            <td>
                                                <i className="fas fa-arrow-down text-warning mr-3"></i> 46,53%
                                            </td>
                                        </tr>
                                        <tr>
                                            <th scope="row">
                                                /argon/charts.html
                                            </th>
                                            <td>
                                                3,513
                                            </td>
                                            <td>
                                                294
                                            </td>
                                            <td>
                                                <i className="fas fa-arrow-down text-warning mr-3"></i> 36,49%
                                            </td>
                                        </tr>
                                        <tr>
                                            <th scope="row">
                                                /argon/tables.html
                                            </th>
                                            <td>
                                                2,050
                                            </td>
                                            <td>
                                                147
                                            </td>
                                            <td>
                                                <i className="fas fa-arrow-up text-success mr-3"></i> 50,87%
                                            </td>
                                        </tr>
                                        <tr>
                                            <th scope="row">
                                                /argon/profile.html
                                            </th>
                                            <td>
                                                1,795
                                            </td>
                                            <td>
                                                190
                                            </td>
                                            <td>
                                                <i className="fas fa-arrow-down text-danger mr-3"></i> 46,53%
                                            </td>
                                        </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div className="col-xl-4">
                            <div className="card shadow">
                                <div className="card-header border-0">
                                    <div className="row align-items-center">
                                        <div className="col">
                                            <h3 className="mb-0">Social traffic</h3>
                                        </div>
                                        <div className="col text-right">
                                            <a href="#!" className="btn btn-sm btn-primary">See all</a>
                                        </div>
                                    </div>
                                </div>
                                <div className="table-responsive">
                                    <!-- Projects table -->
                                    <table className="table align-items-center table-flush">
                                        <thead className="thead-light">
                                        <tr>
                                            <th scope="col">Referral</th>
                                            <th scope="col">Visitors</th>
                                            <th scope="col"></th>
                                        </tr>
                                        </thead>
                                        <tbody>
                                        <tr>
                                            <th scope="row">
                                                Facebook
                                            </th>
                                            <td>
                                                1,480
                                            </td>
                                            <td>
                                                <div className="d-flex align-items-center">
                                                    <span className="mr-2">60%</span>
                                                    <div>
                                                        <div className="progress">
                                                            <div className="progress-bar bg-gradient-danger"
                                                                 role="progressbar"
                                                                 aria-valuenow="60" aria-valuemin="0"
                                                                 aria-valuemax="100"
                                                                 style="width: 60%;"></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th scope="row">
                                                Facebook
                                            </th>
                                            <td>
                                                5,480
                                            </td>
                                            <td>
                                                <div className="d-flex align-items-center">
                                                    <span className="mr-2">70%</span>
                                                    <div>
                                                        <div className="progress">
                                                            <div className="progress-bar bg-gradient-success"
                                                                 role="progressbar"
                                                                 aria-valuenow="70" aria-valuemin="0"
                                                                 aria-valuemax="100"
                                                                 style="width: 70%;"></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th scope="row">
                                                Google
                                            </th>
                                            <td>
                                                4,807
                                            </td>
                                            <td>
                                                <div className="d-flex align-items-center">
                                                    <span className="mr-2">80%</span>
                                                    <div>
                                                        <div className="progress">
                                                            <div className="progress-bar bg-gradient-primary"
                                                                 role="progressbar"
                                                                 aria-valuenow="80" aria-valuemin="0"
                                                                 aria-valuemax="100"
                                                                 style="width: 80%;"></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th scope="row">
                                                Instagram
                                            </th>
                                            <td>
                                                3,678
                                            </td>
                                            <td>
                                                <div className="d-flex align-items-center">
                                                    <span className="mr-2">75%</span>
                                                    <div>
                                                        <div className="progress">
                                                            <div className="progress-bar bg-gradient-info"
                                                                 role="progressbar"
                                                                 aria-valuenow="75" aria-valuemin="0"
                                                                 aria-valuemax="100"
                                                                 style="width: 75%;"></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th scope="row">
                                                twitter
                                            </th>
                                            <td>
                                                2,645
                                            </td>
                                            <td>
                                                <div className="d-flex align-items-center">
                                                    <span className="mr-2">30%</span>
                                                    <div>
                                                        <div className="progress">
                                                            <div className="progress-bar bg-gradient-warning"
                                                                 role="progressbar"
                                                                 aria-valuenow="30" aria-valuemin="0"
                                                                 aria-valuemax="100"
                                                                 style="width: 30%;"></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Footer -->
                    <AdminFooter/>
                </div>
            </div>
        </>
);
}