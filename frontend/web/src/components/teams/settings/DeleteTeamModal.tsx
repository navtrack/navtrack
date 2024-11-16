import { FormattedMessage } from "react-intl";
import { generatePath, useNavigate } from "react-router-dom";
import { useNotification } from "../../ui/notification/useNotification";
import { Paths } from "../../../app/Paths";
import { DeleteModal } from "../../ui/modal/DeleteModal";
import { Button } from "../../ui/button/Button";
import { useCallback } from "react";
import { useDeleteTeamMutation } from "@navtrack/shared/hooks/queries/teams/useDeleteTeamMutation";
import { Team } from "@navtrack/shared/api/model/generated";

export type DeleteAssetFormValues = {
  name?: string; // TODO make required
};

type DeleteTeamModalProps = {
  team?: Team;
};

export function DeleteTeamModal(props: DeleteTeamModalProps) {
  const navigate = useNavigate();
  const { showNotification } = useNotification();
  const deleteTeamMutation = useDeleteTeamMutation({
    organizationId: props.team?.organizationId
  });

  const handleSubmit = useCallback(
    (close: () => void) => {
      if (props.team !== undefined) {
        return deleteTeamMutation.mutateAsync(
          { teamId: props.team.id },
          {
            onSuccess: () => {
              close();
              navigate(
                generatePath(Paths.OrganizationTeams, {
                  id: props.team?.organizationId
                })
              );
              showNotification({
                type: "success",
                description: "assets.settings.general.delete-asset.success"
              });
            }
          }
        );
      }

      return Promise.resolve();
    },
    [deleteTeamMutation, navigate, props.team, showNotification]
  );

  return (
    <DeleteModal
      maxWidth="lg"
      onConfirm={handleSubmit}
      isLoading={deleteTeamMutation.isLoading}
      renderButton={(open) => (
        <Button color="error" type="submit" size="base" onClick={open}>
          <FormattedMessage id="teams.settings.delete.team" />
        </Button>
      )}>
      <div className="mt-2 text-sm">
        <p>
          <FormattedMessage
            id="teams.settings.delete.team.question"
            values={{
              team: <span className="font-semibold">{props.team?.name}</span>
            }}
          />
        </p>
      </div>
    </DeleteModal>
  );
}
