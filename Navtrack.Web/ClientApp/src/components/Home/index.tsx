import React from "react";
import AdminLayout from "components/Framework/Layouts/Admin/AdminLayout";

export default function Home() {
  return (
    <AdminLayout>
      <div className="bg-white shadow p-3 rounded flex">Welcome to Navtrack!</div>
    </AdminLayout>
  );
}
