import { DeviceModel } from "@navtrack/shared/api/model/generated";
import { useDeleteDeviceMutation } from "@navtrack/shared/hooks/mutations/assets/useDeleteDeviceMutation";
import { FormattedMessage, useIntl } from "react-intl";
import { useNotification } from "../../../ui/notification/useNotification";
import { useQueryClient } from "@tanstack/react-query";
import { getAssetsDevicesGetListQueryKey } from "@navtrack/shared/api/index-generated";
import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { ITableColumn } from "../../../ui/table/useTable";
import { TableV1 } from "../../../ui/table/TableV1";
import { DeleteModal } from "../../../ui/modal/DeleteModal";

type AssetDevicesTableProps = {
  rows?: DeviceModel[];
  loading: boolean;
  refresh: () => void;
};

export function AssetDevicesTable(props: AssetDevicesTableProps) {
  const deleteDeviceMutation = useDeleteDeviceMutation();
  const queryClient = useQueryClient();
  const { showNotification } = useNotification();
  const intl = useIntl();
  const currentAsset = useCurrentAsset();

  const columns: ITableColumn<DeviceModel>[] = [
    {
      labelId: "generic.device-type",
      row: (device) => device.deviceType.displayName
    },
    {
      labelId: "generic.serial-number",
      row: (device) => device.serialNumber
    },
    {
      labelId: "generic.status",
      row: (device) => (
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
    { labelId: "generic.positions", row: (device) => device.positions },
    {
      rowClassName: "flex justify-end",
      row: (device) => (
        <>
          {!device.active && !device.positions && (
            <DeleteModal
              isLoading={deleteDeviceMutation.isLoading}
              onConfirm={() => {
                if (currentAsset.data) {
                  return deleteDeviceMutation.mutateAsync(
                    {
                      assetId: currentAsset.data.id,
                      deviceId: device.id
                    },
                    {
                      onSuccess: () => {
                        showNotification({
                          type: "success",
                          description: intl.formatMessage({
                            id: "assets.settings.device.delete.success"
                          })
                        });
                        if (currentAsset.data) {
                          queryClient.refetchQueries(
                            getAssetsDevicesGetListQueryKey(
                              currentAsset.data.id
                            )
                          );
                        }
                      }
                    }
                  );
                }

                return Promise.resolve();
              }}>
              <FormattedMessage
                id="assets.settings.device.delete.question"
                values={{
                  device: (
                    <span className="font-bold">
                      {device.deviceType.displayName}
                    </span>
                  ),
                  serialNumber: (
                    <span className="font-bold">{device.serialNumber}</span>
                  ),
                  assetName: (
                    <span className="font-bold">{currentAsset.data?.name}</span>
                  )
                }}
              />
            </DeleteModal>
          )}
        </>
      )
    }
  ];

  return <TableV1 rows={props.rows} columns={columns} />;
}
