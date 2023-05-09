import { Redirect, Route, Switch } from "react-router-dom";
import { ForgotPasswordPage } from "../components/user/forgot-password/ForgotPasswordPage";
import { LoginPage } from "../components/user/login-page/LoginPage";
import { RegisterPage } from "../components/user/register-page/RegisterPage";
import { ResetPasswordPage } from "../components/user/reset-password-page/ResetPasswordPage";
import { LoginLayout } from "../components/ui/layouts/LoginLayout";
import Paths from "./Paths";

export function RoutesUnauthenticated() {
  return (
    <Switch>
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
      <Route path={Paths.ResetPassword}>
        <LoginLayout>
          <ResetPasswordPage />
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
