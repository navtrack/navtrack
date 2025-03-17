import { ReactNode } from "react";
import { LatLong } from "../api/model";

export function showNumber(value?: number, decimals?: number): ReactNode {
  return value !== undefined
    ? `${decimals ? value.toFixed(decimals) : value}`
    : "-";
}

export function showCoordinate(value?: number): ReactNode {
  return showNumber(value, 6);
}

export function showCoordinates(coordinates: LatLong): string {
  return `${showCoordinate(coordinates.latitude)}, ${showCoordinate(coordinates.longitude)}`;
}

export function showHeading(value?: number | null): ReactNode {
  return value !== undefined && value !== null ? `${value}°` : "-";
}

export function showProperty(value?: string | number | null): ReactNode {
  return value !== undefined && value !== null ? value : "-";
}
