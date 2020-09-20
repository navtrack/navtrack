import React from "react";
import { DeviceModel } from "../../apis/types/device/DeviceModel";

type Props = {
  device: DeviceModel;
};

export default function DeviceLog(props: Props) {
  return (
    <div className="bg-white shadow p-3 rounded mb-3 flex">
      <div className="w-full">
        <div className="text-xl mb-1 pb-2 border-b">Device log</div>
        <div className="pt-2 pb-4"></div>
      </div>
    </div>
  );
}
