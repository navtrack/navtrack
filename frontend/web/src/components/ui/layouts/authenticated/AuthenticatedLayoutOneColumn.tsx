import { Notification } from "../../notification/Notification";
import { AuthenticatedLayoutNavbar } from "./AuthenticatedLayoutNavbar";

type AuthenticatedLayoutOneColumnProps = {
  children?: React.ReactNode;
};

export function AuthenticatedLayoutOneColumn(
  props: AuthenticatedLayoutOneColumnProps
) {
  return (
    <>
      <AuthenticatedLayoutNavbar />
      {props.children}
      <Notification />
    </>
  );
}
