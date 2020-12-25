import React, { ReactNode } from "react";

type Props = {
  children: ReactNode;
};

export default function LoginBox(props: Props) {
  return <div className="bg-white rounded-md px-8 py-6 w-full">{props.children}</div>;
}
