import { Icon } from "../../../ui/icon/Icon";
import { ReactNode } from "react";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { Button } from "../../../ui/button/Button";

type ExternalLoginButtonProps = {
  onClick?: React.MouseEventHandler<HTMLButtonElement>;
  icon: IconProp;
  children: ReactNode;
};

export function ExternalLoginButton(props: ExternalLoginButtonProps) {
  return (
    <Button color="white" onClick={props.onClick} full>
      <Icon icon={props.icon} size="lg" className="mr-2" />
      <span>{props.children}</span>
    </Button>
  );
}
