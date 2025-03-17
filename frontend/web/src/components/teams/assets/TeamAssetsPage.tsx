import { Heading } from "../../ui/heading/Heading";
import { ITableColumn } from "../../ui/table/useTable";
import { TeamAsset } from "@navtrack/shared/api/model";
import { generatePath, Link, useParams } from "react-router-dom";
import { Paths } from "../../../app/Paths";
import { TableV2 } from "../../ui/table/TableV2";
import { TeamLayout } from "../TeamLayout";
import { DeleteModal } from "../../ui/modal/DeleteModal";
import { useNotification } from "../../ui/notification/useNotification";
import { FormattedMessage } from "react-intl";
import { getError } from "@navtrack/shared/utils/api";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { useTeamQuery } from "@navtrack/shared/hooks/queries/teams/useTeamQuery";
import { CreateTeamAssetModal } from "./CreateTeamAssetModal";
import { useTeamAssetsQuery } from "@navtrack/shared/hooks/queries/teams/useTeamAssetsQuery";
import { useDeleteTeamAssetMutation } from "@navtrack/shared/hooks/queries/teams/useDeleteTeamAssetMutation";

export function TeamAssetsPage() {
  const { id } = useParams();
  const assets = useTeamAssetsQuery({ teamId: id });
  const { showNotification } = useNotification();
  const deleteAsset = useDeleteTeamAssetMutation();
  const show = useShow();
  const team = useTeamQuery({ teamId: id });

  const columns: ITableColumn<TeamAsset>[] = [
    {
      labelId: "generic.name",
      row: (asset) => (
        <Link
          to={generatePath(Paths.TeamUsers, { id: asset.assetId })}
          className="text-blue-700">
          {asset.name}
        </Link>
      )
    },
    {
      labelId: "generic.added-on",
      row: (user) => show.date(user.createdDate)
    },
    {
      rowClassName: "flex justify-end space-x-2",
      row: (asset) => (
        <>
          <DeleteModal
            isLoading={deleteAsset.isLoading}
            onConfirm={(close) => {
              if (team.data) {
                return deleteAsset.mutateAsync(
                  {
                    teamId: team.data.id,
                    assetId: asset.assetId
                  },
                  {
                    onSuccess: () => {
                      showNotification({
                        type: "success",
                        description: "teams.assets.delete.success"
                      });
                    },
                    onError: (error) => {
                      const model = getError(error);

                      showNotification({
                        type: "error",
                        description: `${model.message}`
                      });
                    }
                  }
                );
              }
            }}>
            <FormattedMessage
              id="teams.assets.delete.question"
              values={{
                asset: <span className="font-bold">{asset.name}</span>,
                team: <span className="font-bold">{team.data?.name}</span>
              }}
            />
          </DeleteModal>
        </>
      )
    }
  ];

  return (
    <TeamLayout team={team.data} isLoading={team.isLoading}>
      <div className="flex justify-between">
        <Heading type="h1">
          <FormattedMessage id="teams.assets.title" />
        </Heading>
        <CreateTeamAssetModal teamId={id} />
      </div>
      <TableV2 rows={assets.data?.items} columns={columns} />
    </TeamLayout>
  );
}
