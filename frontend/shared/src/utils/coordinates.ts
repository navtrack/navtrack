import { ReactNode } from "react";

export function showNumber(value?: number, decimals?: number): ReactNode {
  return value !== undefined
    ? `${decimals ? value.toFixed(decimals) : value}`
    : "-";
}

export function showCoordinate(value?: number): ReactNode {
  return showNumber(value, 6);
}

export function showHeading(value?: number | null): ReactNode {
  return value !== undefined && value !== null ? `${value}°` : "-";
}

export function showProperty(value?: string | number | null): ReactNode {
  return value !== undefined && value !== null ? value : "-";
}
