import React from "react";
import AssetLayoutNavbar from "./AssetLayoutNavbar";
import classNames from "classnames";

type Props = {
  hidePadding?: boolean;
  children: React.ReactNode;
};

export default function AssetLayout(props: Props) {
  return (
    <>
      <AssetLayoutNavbar />
      <div
        className={classNames("flex flex-col flex-grow", { "pt-5 pl-5 pr-5": !props.hidePadding })}>
        {props.children}
      </div>
    </>
  );
}
