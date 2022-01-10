import { isDevEnv } from "./isDevEnv";

export enum LogLevel {
  DEBUG = "DEBUG",
  INFO = "INFO",
  WARN = "WARN",
  ERROR = "ERROR"
}

export function log(logLevel: LogLevel, ...data: any[]) {
  if (isDevEnv && logLevel === LogLevel.DEBUG) {
    console.log(...data);
  }
}
