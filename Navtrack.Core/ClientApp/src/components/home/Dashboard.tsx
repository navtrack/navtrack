import React from "react";
import PageLayout from "components/framework/PageLayout";

export default function Dashboard() {
  return (
    <PageLayout>
      <div className="bg-white shadow p-3 rounded flex flex-col">
        <div className="font-bold text-xl">Welcome to Navtrack!</div>
        <div className="mt-2">Thank you for registering so early!</div>
        <div>
          Navtrack is not yet officialy released and a lot of things are still not working here,
          please check back later.
        </div>
      </div>
    </PageLayout>
  );
}
