import { faTrashAlt } from "@fortawesome/free-regular-svg-icons";
import { DeviceModel } from "@navtrack/shared/api/model/generated";
import { useDeleteDeviceMutation } from "@navtrack/shared/hooks/mutations/assets/useDeleteDeviceMutation";
import { FormattedMessage, useIntl } from "react-intl";
import { useNotification } from "../../../ui/notification/useNotification";
import { Table, ITableColumn } from "../../../ui/table/Table";
import { useQueryClient } from "@tanstack/react-query";
import { getAssetsDevicesGetListQueryKey } from "@navtrack/shared/api/index-generated";
import { Button } from "../../../ui/button/Button";
import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";

type DevicesTableProps = {
  rows?: DeviceModel[];
  loading: boolean;
  refresh: () => void;
};

export function DevicesTable(props: DevicesTableProps) {
  const deleteDeviceMutation = useDeleteDeviceMutation();
  const queryClient = useQueryClient();
  const { showNotification } = useNotification();
  const intl = useIntl();
  const currentAsset = useCurrentAsset();

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
    { labelId: "generic.locations", render: (device) => device.positions },
    {
      render: (device) => (
        <>
          {!device.active && (
            <Button
              icon={faTrashAlt}
              color="error"
              onClick={() => {
                if (currentAsset.data) {
                  deleteDeviceMutation
                    .mutateAsync(
                      { assetId: currentAsset.data.id, deviceId: device.id },
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
                    .then(() => {
                      if (currentAsset.data) {
                        queryClient.refetchQueries(
                          getAssetsDevicesGetListQueryKey(currentAsset.data.id)
                        );
                      }
                    });
                }
              }}
            />
          )}
        </>
      )
    }
  ];

  return <Table rows={props.rows} loading={props.loading} columns={columns} />;
}
