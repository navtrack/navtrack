import { ReactNode } from "react";

export function showCoordinate(value: number): ReactNode {
  return `${value.toFixed(6)}`;
}

export function showHeading(value?: number | null): ReactNode {
  return value !== undefined && value !== null ? `${value}°` : "-";
}

export function showProperty(value?: string | number | null): ReactNode {
  return value !== undefined && value !== null ? value : "-";
}
