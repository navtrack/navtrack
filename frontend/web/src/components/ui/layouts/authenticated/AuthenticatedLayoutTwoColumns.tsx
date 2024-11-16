import { Notification } from "../../notification/Notification";
import { AuthenticatedLayoutNavbar } from "./AuthenticatedLayoutNavbar";
import { Outlet } from "react-router-dom";
import { AuthenticatedLayoutSidebar } from "./AuthenticatedLayoutSidebar";
import { ReactNode } from "react";
import { LoadingIndicator } from "../../loading-indicator/LoadingIndicator";

type AuthenticatedLayoutTwoColumnsProps = {
  children?: ReactNode;
  isLoading?: boolean;
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
          <div
            className="flex flex-grow flex-col gap-y-4 overflow-y-scroll p-6"
            style={{
              height: "calc(100vh - 112px)"
            }}>
            {props.isLoading ? (
              <div className="flex items-center justify-center">
                <LoadingIndicator size="xl" className="text-gray-900" />
              </div>
            ) : (
              props.children ?? <Outlet />
            )}
          </div>
        </div>
      </div>
      <Notification />
    </>
  );
}
