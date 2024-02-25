import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { ReactNode } from "react";

type SkeletonProps = {
  children: ReactNode;
  loading: boolean;
  className?: string;
  background?: string;
};

export function Skeleton(props: SkeletonProps) {
  if (props.loading) {
    return (
      <div className={classNames("relative overflow-hidden", props.className)}>
        <div className="absolute h-full w-full rounded-md bg-white" />
        <div
          className={classNames(
            "absolute h-full w-full animate-pulse rounded-md",
            c(!!props.background, props.background, "bg-gray-100")
          )}
        />
        {props.children}
      </div>
    );
  }

  return <>{props.children}</>;
}
