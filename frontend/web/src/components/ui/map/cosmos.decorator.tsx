import React, { ReactNode } from "react";

export default ({ children }: { children: ReactNode }) => {
  return <div className="absolute inset-10 flex">{children}</div>;
};
