import { ReactNode } from "react";

type MapContainerProps = {
  children?: ReactNode;
  style?: React.CSSProperties;
};

export function MapContainer(props: MapContainerProps) {
  return (
    <div
      className="flex flex-grow overflow-hidden rounded-md"
      style={props.style}>
      {props.children}
    </div>
  );
}
