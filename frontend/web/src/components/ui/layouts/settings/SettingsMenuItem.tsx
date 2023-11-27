import { FormattedMessage } from "react-intl";
import { Icon } from "../../icon/Icon";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { NavLink } from "react-router-dom";

export type SettingsMenuItemProps = {
  label: string;
  path: string;
  icon: IconProp;
  priority: number;
  subItems?: SettingsMenuItemProps[];
};

export function SettingsMenuItem(props: SettingsMenuItemProps) {
  return (
    <NavLink to={props.path} key={props.label} className="block">
      {({ isActive }) => (
        <div>
          <div
            className={classNames(
              c(
                isActive,
                "bg-gray-50 text-indigo-600",
                "text-gray-900 hover:bg-gray-50 hover:text-indigo-600"
              ),
              "group flex cursor-pointer items-center rounded-md px-3 py-3 text-sm font-medium"
            )}>
            <Icon
              icon={props.icon}
              className={classNames(
                c(
                  isActive,
                  "text-indigo-600 group-hover:text-indigo-600",
                  "text-gray-500 group-hover:text-indigo-600"
                ),
                "-ml-1 mr-3 h-6 w-6 flex-shrink-0"
              )}
            />
            <span className="truncate">
              <FormattedMessage id={props.label} />
            </span>
          </div>
        </div>
      )}
    </NavLink>
  );
}
