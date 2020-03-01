import React, { ReactNode } from "react";
import "leaflet/dist/leaflet.css";
import { LocationModel } from "services/Api/Model/LocationModel";
import classNames from "classnames";
import Icon from "components/Framework/Util/Icon";
import { useOnArrowUp, useOnArrowDown } from "components/hooks/useOnKeyDown";

type Props = {
  loading: boolean;
  locations: LocationModel[];
  selectedLocationIndex: number;
  setSelectedLocationIndex: React.Dispatch<React.SetStateAction<number>>;
};

export default function LocationsTable(props: Props) {
  useOnArrowUp(() => {
    let newIndex = props.selectedLocationIndex - 1;
    if (newIndex >= 0) {
      props.setSelectedLocationIndex(newIndex);
      scrollTableUp(newIndex);
    }
  });

  useOnArrowDown(() => {
    let newIndex = props.selectedLocationIndex + 1;
    if (newIndex < props.locations.length) {
      props.setSelectedLocationIndex(newIndex);
      scrollTableDown(newIndex);
    }
  });

  return (
    <div
      className="bg-white mb-3 overflow-scroll rounded shadow"
      style={{ height: "185px" }}
      id="locationsTable">
      <table className="w-full text-left">
        <thead
          className="text-xs text-gray-700 uppercase font-medium sticky"
          style={{ height: "35px" }}>
          <tr>
            <Th text="Date" />
            <Th text="Latitude" />
            <Th text="Longitude" />
            <Th text="Speed" />
            <Th text="Heading" />
            <Th text="Altitude" />
            <Th text="Satellites" />
            <Th text="HDOP" />
          </tr>
        </thead>
        <tbody className="text-sm" style={{ height: "150px" }}>
          {props.loading ? (
            <tr className="bg-white">
              <td className="p-2 text-center" colSpan={8} style={{ height: "150px" }}>
                <Icon className="fa-spinner  fa-spin text-3xl" />
              </td>
            </tr>
          ) : props.locations.length > 0 ? (
            props.locations.map((x, index) => (
              <tr
                className={classNames("border-b bg-white", {
                  "bg-gray-200": props.selectedLocationIndex === index
                })}
                key={x.id}
                id={`locationRow${index}`}
                onClick={() => props.setSelectedLocationIndex(index)}>
                <Td text={x.dateTime} />
                <Td text={x.latitude} />
                <Td text={x.longitude} />
                <Td text={x.speed} />
                <Td text={x.heading} />
                <Td text={x.altitude} />
                <Td text={x.satellites} />
                <Td text={x.hdop} />
              </tr>
            ))
          ) : (
            <tr className="bg-white">
              <td className="p-2 text-center" colSpan={8} style={{ height: "150px" }}>
                No locations found.
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}

function Th(props: { text: string }) {
  return (
    <th className="p-0 font-medium sticky top-0 z-10 bg-gray-100">
      <div className="border-b p-2">{props.text}</div>
    </th>
  );
}

function Td(props: { text: ReactNode }) {
  return <td className="px-2 py-1">{props.text}</td>;
}

// TODO
function scrollTableUp(newIndex: number) {
  let table = document.getElementById("locationsTable");
  let row = document.getElementById(`locationRow${newIndex}`);
  if (table && row) {
    let rowPos = newIndex * 30;
    let start = table.scrollTop;
    let end = start + 150;
    if (rowPos <= start || rowPos >= end) {
      table.scrollTop = rowPos;
    }
  }
}

function scrollTableDown(newIndex: number) {
  let table = document.getElementById("locationsTable");
  let row = document.getElementById(`locationRow${newIndex}`);
  if (table && row) {
    let rowPos = newIndex * 30;
    let start = table.scrollTop;
    let end = start + 150;
    if (rowPos >= end || rowPos <= start) {
      table.scrollTop = rowPos - 120;
    }
  }
}
