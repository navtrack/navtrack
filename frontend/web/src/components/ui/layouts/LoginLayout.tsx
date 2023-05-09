import { ReactNode } from "react";
import NavtrackLogo from "../../../assets/images/navtrack.svg";
import { useIntl } from "react-intl";

interface ILoginLayoutProps {
  children: ReactNode;
}

export function LoginLayout(props: ILoginLayoutProps) {
  const intl = useIntl();

  return (
    <div className="flex min-h-screen flex-col bg-gray-100 p-8">
      <img
        className="mx-auto w-20 rounded-full bg-gray-800"
        src={NavtrackLogo}
        alt={intl.formatMessage({ id: "navtrack" })}
      />
      {props.children}
    </div>
  );
}
