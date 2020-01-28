import React from "react";
import AdminLayout from "components/Framework/Layouts/Admin/AdminLayout";

export default function Home() {
  return (
    <AdminLayout>
      <div className="row">
        <div className="col-xl-3 col-lg-6">
          <div className="card card-stats mb-4 mb-xl-0">
            <div className="card-body">
              <div className="row">
                <div className="col">
                  <h5 className="card-title text-uppercase text-muted mb-0">Assets Online</h5>
                  <span className="h2 font-weight-bold mb-0">2 out of 3</span>
                </div>
                <div className="col-auto">
                  <div className="icon icon-shape bg-info text-white rounded-circle shadow">
                    <i className="fas fa-plug"></i>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </AdminLayout>
  );
}