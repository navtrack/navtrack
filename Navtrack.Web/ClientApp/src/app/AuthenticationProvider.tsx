import useAxiosAuthorization from "../api/useAxiosAuthorization";
import { useAuthentication } from "../hooks/authentication/useAuthentication";

const AuthenticationProvider: React.FC = (props) => {
  useAxiosAuthorization();
  useAuthentication();

  return <>{props.children}</>;
};

export default AuthenticationProvider;
