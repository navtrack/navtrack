import { ReactNode } from "react";
import { Card } from "./Card";
import { classNames } from "@navtrack/shared/utils/tailwind";

export type DeleteCardProps = {
  children?: ReactNode;
  className?: string;
};

export function DeleteCard(props: DeleteCardProps) {
  return (
    <Card className={classNames("ring-red-500/60", props.className)}>
      {props.children}
    </Card>
  );
}
