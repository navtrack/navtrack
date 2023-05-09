import { ReactNode } from "react";
import { AdminLayoutSideBar } from "./AdminLayoutSideBar";
import { AdminLayoutNavBar } from "./AdminLayoutNavBar";
import { Notification } from "../../shared/notification/Notification";

export interface IAdminLayout {
  children?: ReactNode;
}

export function AdminLayout(props: IAdminLayout) {
  return (
    <>
      <AdminLayoutSideBar />
      <div className="ml-64 flex min-h-screen flex-1 flex-col bg-gray-100">
        <AdminLayoutNavBar />
        <div className="flex flex-grow flex-col gap-y-4 p-4">
          {props.children}
        </div>
      </div>
      <Notification />
    </>
  );
}
