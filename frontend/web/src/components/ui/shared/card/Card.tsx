import { ReactNode } from "react";
import classNames from "classnames";

export interface ICard {
  children: ReactNode;
  className?: string;
}

export function Card(props: ICard) {
  return (
    <div className={classNames("rounded-lg bg-white shadow", props.className)}>
      {props.children}
    </div>
  );
}
