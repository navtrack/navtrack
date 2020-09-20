import React, { useState, useEffect } from "react";
import { DeviceApi } from "../../../../apis/DeviceApi";
import { AssetModel } from "../../../../apis/types/asset/AssetModel";
import { DeviceConnectionResponseModel } from "../../../../apis/types/device/DeviceConnectionResponseModel";
import { DeviceModel } from "../../../../apis/types/device/DeviceModel";
import { ResultsPaginationModel } from "../../../../apis/types/ResultsPaginationModel";
import { showDate } from "../../../../services/util/DateUtil";
import Table, { TableColumn } from "../../../shared/table/Table";

type Props = {
  asset: AssetModel;
  device: DeviceModel;
};

export default function AssetSettingsDeviceConnections(props: Props) {
  const [currentPage, setCurrentPage] = useState(1);
  const [connections, setConnections] = useState<
    ResultsPaginationModel<DeviceConnectionResponseModel>
  >();

  useEffect(() => {
    DeviceApi.connections(props.device.id, currentPage).then((x) => {
      setConnections(x);
    });
  }, [currentPage, props.device.id]);

  const columns: TableColumn<DeviceConnectionResponseModel>[] = [
    {
      title: "Opened at",
      renderer: (x) => showDate(x.openedAt)
    },
    {
      title: "Closed at",
      renderer: (x) => showDate(x.closedAt)
    },
    {
      title: "Remote Endpoint",
      renderer: (x) => x.remoteEndPoint
    },
    {
      title: "Messages",
      renderer: (x) => x.messages
    }
  ];

  return (
    <>
      {connections && (
        <Table<DeviceConnectionResponseModel>
          columns={columns}
          data={connections}
          currentPage={currentPage}
          setCurrentPage={setCurrentPage}
        />
      )}
    </>
  );
}
