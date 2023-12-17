import React, { ReactNode } from "react";

export default ({ children }: { children: ReactNode }) => {
  return <div className="absolute inset-0 flex">{children}</div>;
};
