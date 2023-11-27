import { classNames } from "@navtrack/shared/utils/tailwind";
import { NavtrackLogo } from "./NavtrackLogo";

type NavtrackLogoProps = {
  className?: string;
};

export function NavtrackLogoDark(props: NavtrackLogoProps) {
  return (
    <div
      className={classNames("flex rounded-full bg-gray-900", props.className)}>
      <NavtrackLogo />
    </div>
  );
}
