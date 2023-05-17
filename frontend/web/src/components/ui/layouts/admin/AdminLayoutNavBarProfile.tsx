import { Fragment } from "react";
import { Menu, Transition } from "@headlessui/react";
import {
  faSignOutAlt,
  faSlidersH,
  faUser
} from "@fortawesome/free-solid-svg-icons";
import { Icon } from "../../shared/icon/Icon";
import { FormattedMessage } from "react-intl";
import { useHistory } from "react-router-dom";
import { IconWithText } from "../../shared/icon/IconWithText";
import { useLogout } from "@navtrack/shared/hooks/app/authentication/useLogout";
import { useCurrentUser } from "@navtrack/shared/hooks/user/useCurrentUser";
import { classNames } from "@navtrack/shared/utils/tailwind";

export type AdminLayoutNavBarProfileProps = {};

export function AdminLayoutNavBarProfile(props: AdminLayoutNavBarProfileProps) {
  const currentUser = useCurrentUser();
  const logout = useLogout();
  const history = useHistory();

  return (
    <Menu as="div" className="relative ml-3 flex">
      <Menu.Button className="flex items-center rounded-full text-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-4">
        <Icon icon={faUser} />
      </Menu.Button>
      <Transition
        as={Fragment}
        enter="transition ease-out duration-100"
        enterFrom="transform opacity-0 scale-95"
        enterTo="transform opacity-100 scale-100"
        leave="transition ease-in duration-75"
        leaveFrom="transform opacity-100 scale-100"
        leaveTo="transform opacity-0 scale-95">
        <Menu.Items className="absolute right-0 mt-6 origin-top-right divide-y divide-gray-100 rounded-md bg-white py-1 shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none">
          <div className="py-1">
            <Menu.Item disabled>
              <div className="cursor-default bg-white px-4 py-2 text-sm text-gray-700">
                <FormattedMessage id="navbar.profile.logged-in-as" />{" "}
                <span className="font-semibold">{currentUser?.email}</span>
              </div>
            </Menu.Item>
          </div>
          <div className="py-1">
            <Menu.Item>
              {({ active }) => (
                <div
                  onClick={() => history.push("/settings/account")}
                  className={classNames(
                    active ? "bg-gray-100" : "",
                    "block cursor-pointer px-4 py-2 text-sm text-gray-700"
                  )}>
                  <IconWithText icon={faSlidersH}>
                    <FormattedMessage id="navbar.profile.settings" />
                  </IconWithText>
                </div>
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
                    <FormattedMessage id="navbar.profile.logout" />
                  </IconWithText>
                </div>
              )}
            </Menu.Item>
          </div>
        </Menu.Items>
      </Transition>
    </Menu>
  );
}
