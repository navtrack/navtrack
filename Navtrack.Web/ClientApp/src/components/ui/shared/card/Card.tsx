import { ReactNode } from "react";
import classNames from "classnames";

export interface ICard {
  children: ReactNode;
  className?: string;
}

export default function Card(props: ICard) {
  return (
    <div className={classNames("shadow rounded-lg bg-white", props.className)}>
      {props.children}
    </div>
  );
}
