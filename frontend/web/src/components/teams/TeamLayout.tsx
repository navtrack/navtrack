import { generatePath, useParams } from "react-router-dom";
import { Paths } from "../../app/Paths";
import { AuthenticatedLayoutTwoColumns } from "../ui/layouts/authenticated/AuthenticatedLayoutTwoColumns";
import { Heading } from "../ui/heading/Heading";
import { TabMenuItemProps, TabsLayout } from "../ui/layouts/tabs/TabsLayout";
import { TeamModel } from "@navtrack/shared/api/model";
import { Skeleton } from "../../../../shared/src/components/components/ui/skeleton/Skeleton";
import { FormattedMessage } from "react-intl";
import { useTeamQuery } from "@navtrack/shared/hooks/queries/teams/useTeamQuery";

export type TeamLayoutProps = {
  team?: TeamModel;
  isLoading?: boolean;
  children?: React.ReactNode;
};

export function TeamLayout(props: TeamLayoutProps) {
  const { id } = useParams();
  const team = useTeamQuery({ teamId: id });

  const tabs: TabMenuItemProps[] = [
    {
      name: "Users",
      href: generatePath(Paths.TeamUsers, { id }),
      count: team.data?.usersCount
    },
    {
      name: "Assets",
      href: generatePath(Paths.TeamAssets, { id }),
      count: team.data?.assetsCount
    },
    {
      name: "Settings",
      href: generatePath(Paths.TeamSettings, { id })
    }
  ];

  return (
    <AuthenticatedLayoutTwoColumns>
      <div>
        <div className="flex">
          <Skeleton isLoading={props.isLoading}>
            <Heading type="h1">
              {props.isLoading ? (
                <FormattedMessage id="generic.loading" />
              ) : (
                props.team?.name
              )}
            </Heading>
          </Skeleton>
        </div>
        <TabsLayout tabs={tabs} />
      </div>
      <div className="flex flex-1 flex-col gap-x-6 space-y-5">
        {props.children}
      </div>
    </AuthenticatedLayoutTwoColumns>
  );
}
