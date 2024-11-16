import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { ReactNode } from "react";
import { LoadingIndicator } from "../loading-indicator/LoadingIndicator";

type SkeletonProps = {
  children: ReactNode;
  isLoading?: boolean;
  className?: string;
  background?: string;
  indicator?: boolean;
};

export function Skeleton(props: SkeletonProps) {
  if (props.isLoading) {
    if (props.indicator) {
      return (
        <div
          className={classNames(
            "flex items-center justify-center",
            props.className
          )}>
          <LoadingIndicator size="lg" className="p-4" />
        </div>
      );
    }
    return (
      <div className={classNames("relative overflow-hidden", props.className)}>
        <div className="absolute h-full w-full rounded-md bg-gray-200" />
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
