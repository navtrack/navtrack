import { isDevEnv } from "./isDevEnv";

export function log(...data: any[]) {
  if (isDevEnv) {
    console.log(...data);
  }
}
