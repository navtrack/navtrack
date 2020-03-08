import React from "react";
import AdminLayout from "../Admin/AdminLayout";
import AssetLayoutNavbar from "./AssetLayoutNavbar";

type Props = {
  children: React.ReactNode;
};

export default function AssetLayout(props: Props) {
  return (
    <AdminLayout hidePadding={true}>
      <AssetLayoutNavbar />
      <div className="pt-5 pl-5 pr-5 flex flex-col flex-grow">{props.children}</div>
    </AdminLayout>
  );
}
