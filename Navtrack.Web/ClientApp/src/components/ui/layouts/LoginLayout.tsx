import { ReactNode } from "react";
import NavtrackLogo from "../../../assets/images/navtrack.svg";
import { useIntl } from "react-intl";

interface ILoginLayoutProps {
  children: ReactNode;
}

export default function LoginLayout(props: ILoginLayoutProps) {
  const intl = useIntl();

  return (
    <div className="bg-gray-100 min-h-screen flex flex-col p-8">
      <img
        className="mx-auto w-20 bg-gray-800 rounded-full"
        src={NavtrackLogo}
        alt={intl.formatMessage({ id: "navtrack" })}
      />
      {props.children}
    </div>
  );
}
