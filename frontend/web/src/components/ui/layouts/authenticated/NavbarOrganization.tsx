import { Fragment, useState } from "react";
import { Menu, Transition } from "@headlessui/react";
import { classNames } from "@navtrack/shared/utils/tailwind";
import { Icon } from "../../icon/Icon";
import { faAngleDown, faPlus } from "@fortawesome/free-solid-svg-icons";
import { FormattedMessage } from "react-intl";
import { IconWithText } from "../../icon/IconWithText";
import { generatePath, useNavigate } from "react-router-dom";
import { Paths } from "../../../../app/Paths";
import { faBuilding } from "@fortawesome/free-regular-svg-icons";
import { CreateOrganizationModal } from "./CreateOrganizationModal";
import { Badge, BadgeColor } from "../../badge/Badge";
import { LoadingIndicator } from "../../loading-indicator/LoadingIndicator";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";
import { useOrganizationsQuery } from "@navtrack/shared/hooks/queries/organizations/useOrganizationsQuery";

export function NavbarOrganization() {
  const currentOrganization = useCurrentOrganization();
  const organizations = useOrganizationsQuery();
  const navigate = useNavigate();
  const [open, setOpen] = useState(false);

  return (
    <>
      <CreateOrganizationModal open={open} setOpen={setOpen} />
      <Menu as="div" className="relative">
        <div>
          <Menu.Button
            disabled={currentOrganization.isLoading}
            className="relative flex rounded-md bg-white text-sm text-gray-900 hover:text-gray-500 focus:outline-none focus:ring-2 focus:ring-blue-700 focus:ring-offset-2 disabled:hover:text-gray-900">
            <Badge
              color={BadgeColor.Gray}
              size="lg"
              className="hover:bg-gray-100">
              {currentOrganization.isLoading ? (
                <LoadingIndicator />
              ) : (
                <>
                  <Icon icon={faBuilding} className="mr-2" size="sm" />
                  {currentOrganization.data?.name ?? (
                    <FormattedMessage id="organizations.select" />
                  )}
                  <Icon icon={faAngleDown} className="ml-2" size="sm" />
                </>
              )}
            </Badge>
          </Menu.Button>
        </div>
        <Transition
          as={Fragment}
          enter="transition ease-out duration-200"
          enterFrom="transform opacity-0 scale-95"
          enterTo="transform opacity-100 scale-100"
          leave="transition ease-in duration-75"
          leaveFrom="transform opacity-100 scale-100"
          leaveTo="transform opacity-0 scale-95">
          <Menu.Items className="absolute left-0 z-20 mt-2 w-48 origin-top-left rounded-md bg-white py-1 shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none">
            <div className="mb-1 border-b border-gray-200 pb-1">
              <Menu.Item>
                {({ active }) => (
                  <div
                    onClick={(e) => {
                      setOpen(true);
                    }}
                    className={classNames(
                      active ? "bg-gray-100" : "",
                      "block cursor-pointer px-4 py-2 text-sm text-gray-700"
                    )}>
                    <IconWithText icon={faPlus}>
                      <FormattedMessage id="generic.new-organization" />
                    </IconWithText>
                  </div>
                )}
              </Menu.Item>
            </div>
            {organizations.data?.items.map((organization) => (
              <Menu.Item key={organization.id}>
                {({ active }) => (
                  <button
                    className={classNames(
                      active ? "bg-gray-100" : "",
                      " w-full px-4 py-2 text-left text-sm text-gray-700"
                    )}
                    onClick={() => {
                      navigate(
                        generatePath(Paths.OrganizationLive, {
                          id: organization.id
                        })
                      );
                    }}>
                    <IconWithText icon={faBuilding}>
                      {organization.name}
                    </IconWithText>
                  </button>
                )}
              </Menu.Item>
            ))}
          </Menu.Items>
        </Transition>
      </Menu>
    </>
  );
}
