import React from "react";
import useMap from "../../services/hooks/useMap";
import PageLayout from "../shared/PageLayout";

export default function Dashboard() {
  useMap(false);

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
