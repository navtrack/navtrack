import { ReactNode } from "react";
import { NavtrackLogoDark } from "../../logo/NavtrackLogoDark";

type UnauthenticatedLayoutProps = {
  children: ReactNode;
};

export function UnauthenticatedLayout(props: UnauthenticatedLayoutProps) {
  return (
    <div className="flex min-h-screen flex-col bg-gray-100 p-8">
      <div className="mx-auto">
        <NavtrackLogoDark className="w-28 p-4" />
      </div>
      {props.children}
    </div>
  );
}
