import Button from "../../ui/shared/button/Button";
import Icon from "../../ui/shared/icon/Icon";
import { ReactNode } from "react";
import { IconProp } from "@fortawesome/fontawesome-svg-core";

interface IExternalLoginButton {
  onClick?: React.MouseEventHandler<HTMLButtonElement>;
  icon: IconProp;
  children: ReactNode;
}

export function ExternalLoginButton(props: IExternalLoginButton) {
  return (
    <Button color="white" onClick={props.onClick} fullWidth>
      <Icon icon={props.icon} size="lg" className="mr-2" />
      <span>{props.children}</span>
    </Button>
  );
}
