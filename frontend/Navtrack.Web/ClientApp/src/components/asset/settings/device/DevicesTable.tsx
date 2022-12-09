import { faTrashAlt } from "@fortawesome/free-regular-svg-icons";
import {
  getGetAssetsAssetIdDevicesQueryKey,
  useDeleteDeviceMutation
} from "@navtrack/navtrack-app-shared";
import { DeviceModel } from "@navtrack/navtrack-app-shared/dist/api/model/generated";
import { FormattedMessage, useIntl } from "react-intl";
import { useQueryClient } from "react-query";
import IconButton from "../../../ui/shared/button/IconButton";
import useNotification from "../../../ui/shared/notification/useNotification";
import Table, { ITableColumn } from "../../../ui/shared/table/Table";

interface IDevicesTable {
  assetId: string;
  rows?: DeviceModel[];
  loading: boolean;
  refresh: () => void;
}

export default function DevicesTable(props: IDevicesTable) {
  const deleteDeviceMutation = useDeleteDeviceMutation();
  const queryClient = useQueryClient();
  const { showNotification } = useNotification();
  const intl = useIntl();

  const columns: ITableColumn<DeviceModel>[] = [
    {
      labelId: "generic.device-type",
      render: (device) => device.deviceType.displayName
    },
    {
      labelId: "generic.serial-number",
      render: (device) => device.serialNumber
    },
    {
      labelId: "generic.status",
      render: (device) => (
        <>
          {device.active ? (
            <span className="inline-flex items-center rounded-full bg-green-100 px-3 py-0.5 text-sm font-medium text-green-800">
              <FormattedMessage id="generic.active" />
            </span>
          ) : (
            <span className="inline-flex items-center rounded-full bg-gray-100 px-3 py-0.5 text-sm font-medium text-gray-800">
              <FormattedMessage id="generic.inactive" />
            </span>
          )}
        </>
      )
    },
    { labelId: "generic.locations", render: (device) => device.locations },
    {
      render: (device) => (
        <>
          {!device.active && (
            <IconButton
              icon={faTrashAlt}
              className="text-red-500"
              onClick={() =>
                deleteDeviceMutation
                  .mutateAsync(
                    { assetId: props.assetId, deviceId: device.id },
                    {
                      onSuccess: () => {
                        showNotification({
                          type: "success",
                          description: intl.formatMessage({
                            id: "assets.settings.device.delete.success"
                          })
                        });
                      }
                    }
                  )
                  .then(() =>
                    queryClient.refetchQueries(
                      getGetAssetsAssetIdDevicesQueryKey(props.assetId)
                    )
                  )
              }
            />
          )}
        </>
      )
    }
  ];

  return <Table rows={props.rows} loading={props.loading} columns={columns} />;
}
