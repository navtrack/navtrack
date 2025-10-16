// Tremor cx [v0.0.0]

import clsx, { type ClassValue } from "clsx";

export function cx(...args: ClassValue[]) {
  return clsx(...args);
}
