import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { FormattedMessage } from "react-intl";
import { IconWithText } from "../../icon/IconWithText";
import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { NavLink } from "react-router-dom";

type AuthenticatedLayoutNavbarItemProps = {
  labelId: string;
  icon: IconProp;
  to: string;
};

export function AuthenticatedLayoutNavbarItem(
  props: AuthenticatedLayoutNavbarItemProps
) {
  return (
    <NavLink
      to={props.to}
      className={({ isActive }) =>
        classNames(
          c(
            isActive,
            "border-indigo-500 text-gray-900 hover:border-indigo-500"
          ),
          "inline-flex items-center border-b-2 border-transparent px-1 pt-1 text-sm font-medium text-gray-900 hover:border-gray-900"
        )
      }>
      <IconWithText icon={props.icon}>
        <FormattedMessage id={props.labelId} />
      </IconWithText>
    </NavLink>
  );
}
