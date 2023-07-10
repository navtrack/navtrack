import { ReactNode } from "react";
import { NavtrackLogo } from "../NavtrackLogo";

type LoginLayoutProps = {
  children: ReactNode;
};

export function LoginLayout(props: LoginLayoutProps) {
  return (
    <div className="flex min-h-screen flex-col bg-gray-100 p-8">
      <div className="mx-auto flex">
        <div className="flex w-28 rounded-full bg-gray-800">
          <NavtrackLogo />
        </div>
      </div>
      {props.children}
    </div>
  );
}
