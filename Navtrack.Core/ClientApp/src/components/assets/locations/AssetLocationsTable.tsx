import React from "react";
import { LocationModel } from "../../../apis/types/location/LocationModel";
import { useOnArrowUp, useOnArrowDown } from "../../../services/hooks/useOnKeyDown";
import { showDate } from "../../../services/util/DateUtil";
import { ScrollTableUtil } from "../../shared/table/scrollable/ScrollableTableUtil";
import ScrollableTableHeader from "../../shared/table/scrollable/ScrollableTableHeader";
import ScrollableTable from "../../shared/table/scrollable/ScrollableTable";
import ScrollableTableRow from "../../shared/table/scrollable/ScrollableTableRow";
import ScrollableTableBody from "../../shared/table/scrollable/ScrollableTableBody";
import ScrollableTableFooter from "../../shared/table/scrollable/ScrollableTableFooter";

type Props = {
  loading: boolean;
  locations: LocationModel[];
  selectedIndex: number;
  setSelectedIndex: React.Dispatch<React.SetStateAction<number>>;
};

export default function AssetLocationsTable(props: Props) {
  useOnArrowUp(() => {
    let newIndex = props.selectedIndex - 1;
    if (newIndex >= 0) {
      props.setSelectedIndex(newIndex);
      ScrollTableUtil.scrollTableUp(newIndex);
    }
  });

  useOnArrowDown(() => {
    let newIndex = props.selectedIndex + 1;
    if (newIndex < props.locations.length) {
      props.setSelectedIndex(newIndex);
      ScrollTableUtil.scrollTableDown(newIndex);
    }
  });

  return (
    <ScrollableTable>
      <ScrollableTableHeader>
        <div style={{ flex: "1 0 80px" }} className="pr-1">
          Date
        </div>
        <div style={{ flex: "1 0" }} className="pr-1">
          Latitude
        </div>
        <div style={{ flex: "1 0" }} className="pr-1">
          Longitude
        </div>
        <div style={{ flex: "1 0" }} className="pr-1">
          Speed
        </div>
        <div style={{ flex: "1 0" }} className="pr-1">
          Heading
        </div>
        <div style={{ flex: "1 0" }} className="pr-1">
          Altitude
        </div>
        <div style={{ flex: "1 0" }} className="pr-1">
          Satellites
        </div>
        <div style={{ flex: "1 0" }} className="pr-1">
          HDOP
        </div>
      </ScrollableTableHeader>
      <ScrollableTableBody>
        {props.locations.map((x, index) => (
          <ScrollableTableRow
            onClick={() => props.setSelectedIndex(index)}
            index={index}
            selectedIndex={props.selectedIndex}
            length={props.locations.length}
            key={index}>
            <div style={{ flex: "1 0 80px" }}>{showDate(x.dateTime)}</div>
            <div style={{ flex: "1 0" }}>{x.latitude}</div>
            <div style={{ flex: "1 0" }}>{x.longitude}</div>
            <div style={{ flex: "1 0" }}>{x.speed}</div>
            <div style={{ flex: "1 0" }}>{x.heading}</div>
            <div style={{ flex: "1 0" }}>{x.altitude}</div>
            <div style={{ flex: "1 0" }}>{x.satellites}</div>
            <div style={{ flex: "1 0" }}>{x.hdop}</div>
          </ScrollableTableRow>
        ))}
      </ScrollableTableBody>
      <ScrollableTableFooter>
        <div style={{ flex: "0 0 80px" }} className="pr-1">
          <span className="font-medium">{props.locations.length}</span> locations
        </div>
      </ScrollableTableFooter>
    </ScrollableTable>
  );
}
