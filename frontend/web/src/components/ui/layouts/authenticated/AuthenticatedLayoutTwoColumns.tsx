import { Notification } from "../../notification/Notification";
import { AuthenticatedLayoutNavbar } from "./AuthenticatedLayoutNavbar";
import { Outlet } from "react-router-dom";
import { AuthenticatedLayoutSidebar } from "./AuthenticatedLayoutSidebar";
import { ReactNode } from "react";

type AuthenticatedLayoutTwoColumnsProps = {
  children?: ReactNode;
};

export function AuthenticatedLayoutTwoColumns(
  props: AuthenticatedLayoutTwoColumnsProps
) {
  return (
    <>
      <div className="flex min-h-screen">
        <AuthenticatedLayoutSidebar />
        <div className="ml-64 flex flex-1 flex-col">
          <AuthenticatedLayoutNavbar hideLogo />
          <div className="flex flex-grow flex-col gap-y-4 p-6">
            {props.children ?? <Outlet />}
          </div>
        </div>
      </div>
      <Notification />
    </>
  );
}
