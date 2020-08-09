import React, { ReactNode } from "react";

type Props = {
  children: ReactNode;
  links?: ReactNode;
};

export default function LoginBox(props: Props) {
  return (
    <>
      <div className="shadow-lg bg-white rounded-md px-8 w-full">{props.children}</div>
      {props.links && <div className="h-20 flex w-full">{props.links}</div>}
    </>
  );
}
