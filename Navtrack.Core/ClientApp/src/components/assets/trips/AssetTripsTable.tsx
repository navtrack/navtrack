import React from "react";
import { TripModel } from "../../../apis/types/asset/TripModel";
import { useOnArrowUp, useOnArrowDown } from "../../../services/hooks/useOnKeyDown";
import ScrollableTable from "../../shared/table/scrollable/ScrollableTable";
import ScrollableTableHeader from "../../shared/table/scrollable/ScrollableTableHeader";
import ScrollableTableFooter from "../../shared/table/scrollable/ScrollableTableFooter";
import ScrollableTableBody from "../../shared/table/scrollable/ScrollableTableBody";
import ScrollableTableRow from "../../shared/table/scrollable/ScrollableTableRow";
import { ScrollTableUtil } from "../../shared/table/scrollable/ScrollableTableUtil";

type Props = {
  loading: boolean;
  trips: TripModel[];
  totalDistance: number;
  selectedIndex: number;
  setSelectedIndex: React.Dispatch<React.SetStateAction<number>>;
};

export default function AssetTripsTable(props: Props) {
  useOnArrowUp(() => {
    let newIndex = props.selectedIndex - 1;
    if (newIndex >= 0) {
      props.setSelectedIndex(newIndex);
      ScrollTableUtil.scrollTableUp(newIndex);
    }
  });

  useOnArrowDown(() => {
    let newIndex = props.selectedIndex + 1;
    if (newIndex < props.trips.length) {
      props.setSelectedIndex(newIndex);
      ScrollTableUtil.scrollTableDown(newIndex);
    }
  });

  return (
    <ScrollableTable>
      <ScrollableTableHeader>
        <div style={{ flex: "0 0 80px" }} className="pr-1">
          Trip No.
        </div>
        <div style={{ flex: "1 0" }}>Start time</div>
        <div style={{ flex: "1 0" }}>End time</div>
        <div style={{ flex: "1 0" }}>Distance</div>
      </ScrollableTableHeader>
      <ScrollableTableBody>
        {props.trips.map((x, index) => (
          <ScrollableTableRow
            onClick={() => props.setSelectedIndex(index)}
            index={index}
            selectedIndex={props.selectedIndex}
            length={props.trips.length}
            key={index}>
            <div style={{ flex: "0 0 80px" }} className="pr-1">
              {x.number}
            </div>
            <div style={{ flex: "1 0" }}>{x.startDate}</div>
            <div style={{ flex: "1 0" }}>{x.endDate}</div>
            <div style={{ flex: "1 0" }}>{getDistance(x.distance)}</div>
          </ScrollableTableRow>
        ))}
      </ScrollableTableBody>
      <ScrollableTableFooter>
        <div style={{ flex: "0 0 80px" }} className="pr-1">
          <span className="font-medium">{props.trips.length}</span> trips
        </div>
        <div style={{ flex: "1 0" }}></div>
        <div style={{ flex: "1 0" }} className="text-right mr-1"></div>
        <div style={{ flex: "1 0" }}>
          <span className="font-medium">{getDistance(props.totalDistance)}</span> total
        </div>
      </ScrollableTableFooter>
    </ScrollableTable>
  );
}

function getDistance(distance: number): string {
  if (distance > 1000) {
    return `${Math.round((distance / 1000) * 100) / 100} km`;
  }

  return `${Math.round(distance * 100) / 100} m`;
}
