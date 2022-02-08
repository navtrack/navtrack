import { Redirect, Route, Switch } from "react-router-dom";
import ForgotPasswordPage from "../components/forgot-password/ForgotPasswordPage";
import LoginPage from "../components/login/LoginPage";
import RegisterPage from "../components/register/RegisterPage";
import LoginLayout from "../components/ui/layouts/LoginLayout";
import Paths from "./Paths";

export default function RoutesUnauthenticated() {
  return (
    <LoginLayout>
      <Switch>
        <Route path={Paths.Register}>
          <RegisterPage />
        </Route>
        <Route path={Paths.ForgotPassword}>
          <ForgotPasswordPage />
        </Route>
        <Route path={Paths.Home} exact>
          <LoginPage />
        </Route>
        <Redirect to={Paths.Home} />
      </Switch>
    </LoginLayout>
  );
}
