import React from "react";
import AssetLayoutNavbar from "./AssetLayoutNavbar";

type Props = {
  children: React.ReactNode;
};

export default function AssetLayout(props: Props) {
  return (
    <>
      <AssetLayoutNavbar />
      <div className="pt-5 pl-5 pr-5 flex flex-col flex-grow">{props.children}</div>
    </>
  );
}
