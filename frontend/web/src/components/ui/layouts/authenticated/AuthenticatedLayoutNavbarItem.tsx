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
      className={({ isActive }) => {
        console.log(props.to, isActive);

        return classNames(
          c(
            isActive,
            "border-blue-700 text-gray-900 hover:border-blue-700",
            "border-transparent hover:border-gray-300"
          ),
          "inline-flex items-center border-b-2 px-1 pt-1 text-sm font-medium text-gray-900"
        );
      }}>
      <IconWithText icon={props.icon}>
        <FormattedMessage id={props.labelId} />
      </IconWithText>
    </NavLink>
  );
}
