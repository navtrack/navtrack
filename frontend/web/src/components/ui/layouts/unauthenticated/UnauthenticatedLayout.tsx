import { ReactNode } from "react";
import { NavtrackLogoDark } from "../../logo/NavtrackLogoDark";
import { Link } from "react-router-dom";
import { Paths } from "../../../../app/Paths";

type UnauthenticatedLayoutProps = {
  children: ReactNode;
};

export function UnauthenticatedLayout(props: UnauthenticatedLayoutProps) {
  return (
    <div className="flex min-h-screen flex-col bg-gray-100 p-8">
      <div className="mx-auto">
        <Link to={Paths.Home}>
          <NavtrackLogoDark className="w-28 p-4" />
        </Link>
      </div>
      {props.children}
    </div>
  );
}
