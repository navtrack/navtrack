import { useAuthentication } from "../hooks/authentication/useAuthentication";

const AuthenticationProvider: React.FC = (props) => {
  useAuthentication();

  return <>{props.children}</>;
};

export default AuthenticationProvider;
