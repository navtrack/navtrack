import React, { ReactNode } from "react";

type Props = {
  children: ReactNode;
};

export default function LoginLayout(props: Props) {
  return (
    <div className="bg-gray-800 flex min-h-screen items-center justify-center flex-col">
      <div className="max-w-xs w-full flex flex-col items-center">
        <div className="h-16 m-3 ">
          <a href="https://www.navtrack.io">
            <img src="/navtrack.png" width="64" className="mb-4" alt="Navtrack" />
          </a>
        </div>
        {props.children}
      </div>
    </div>
  );
}
