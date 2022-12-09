import { Redirect, Route, Switch } from "react-router-dom";
import ForgotPasswordPage from "../components/forgot-password/ForgotPasswordPage";
import LoginPage from "../components/login/LoginPage";
import { Maps } from "../components/maps/Maps";
import RegisterPage from "../components/register/RegisterPage";
import LoginLayout from "../components/ui/layouts/LoginLayout";
import Paths from "./Paths";

export default function RoutesUnauthenticated() {
  return (
    <Switch>
      <Route path={Paths.Maps} exact>
        <Maps />
      </Route>
      <Route path={Paths.Register}>
        <LoginLayout>
          <RegisterPage />
        </LoginLayout>
      </Route>
      <Route path={Paths.ForgotPassword}>
        <LoginLayout>
          <ForgotPasswordPage />
        </LoginLayout>
      </Route>
      <Route path={Paths.Home} exact>
        <LoginLayout>
          <LoginPage />
        </LoginLayout>
      </Route>
      <Redirect to={Paths.Home} />
    </Switch>
  );
}
