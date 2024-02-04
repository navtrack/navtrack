import { Fragment } from "react";
import { Menu, Transition } from "@headlessui/react";
import { classNames } from "@navtrack/shared/utils/tailwind";
import { Icon } from "../../icon/Icon";
import { faSignOutAlt, faSlidersH } from "@fortawesome/free-solid-svg-icons";
import { FormattedMessage } from "react-intl";
import { IconWithText } from "../../icon/IconWithText";
import { Link } from "react-router-dom";
import { Paths } from "../../../../app/Paths";
import { useLogin } from "@navtrack/shared/hooks/app/authentication/useLogin";
import { faUser } from "@fortawesome/free-regular-svg-icons";
import { useCurrentUserQuery } from "@navtrack/shared/hooks/queries/useCurrentUserQuery";

export function AuthenticatedLayoutNavbarProfile() {
  const currentUser = useCurrentUserQuery();
  const { logout } = useLogin();

  return (
    <Menu as="div" className="relative">
      <div>
        <Menu.Button className="relative flex h-8 w-8 rounded-full bg-white p-2 text-sm text-gray-900 hover:text-gray-500 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2">
          <Icon icon={faUser} size="lg" />
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
        <Menu.Items className="absolute right-0 z-20 mt-2 w-48 origin-top-right rounded-md bg-white py-1 shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none">
          <Menu.Item disabled>
            <div className="py-1">
              <div className="cursor-default bg-white px-4 py-2 text-sm text-gray-700">
                <FormattedMessage id="navbar.profile.logged-in-as" />{" "}
                <span className="font-semibold">{currentUser.data?.email}</span>
              </div>
            </div>
          </Menu.Item>
          <Menu.Item>
            {({ active }) => (
              <Link
                to={Paths.SettingsAccount}
                className={classNames(
                  active ? "bg-gray-100" : "",
                  "block px-4 py-2 text-sm text-gray-700"
                )}>
                <IconWithText icon={faSlidersH}>
                  <FormattedMessage id="navbar.profile.settings" />
                </IconWithText>
              </Link>
            )}
          </Menu.Item>
          <Menu.Item>
            {({ active }) => (
              <div
                onClick={logout}
                className={classNames(
                  active ? "bg-gray-100" : "",
                  "block cursor-pointer px-4 py-2 text-sm text-gray-700"
                )}>
                <IconWithText icon={faSignOutAlt}>
                  <FormattedMessage id="generic.log-out" />
                </IconWithText>
              </div>
            )}
          </Menu.Item>
        </Menu.Items>
      </Transition>
    </Menu>
  );
}
