import {
  useAuthentication,
  useAxiosAuthorization
} from "@navtrack/navtrack-app-shared";
import { AUTHENTICATION } from "../constants";

const AuthenticationProvider: React.FC = (props) => {
  useAxiosAuthorization();
  useAuthentication({
    clientId: AUTHENTICATION.CLIENT_ID
  });

  return <>{props.children}</>;
};

export default AuthenticationProvider;
