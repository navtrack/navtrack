import { ReactNode } from "react";
import useSentry from "../hooks/sentry/useSentry";

export const SentryProvider = (props: { children: ReactNode }) => {
  useSentry();

  return <>{props.children}</>;
};
