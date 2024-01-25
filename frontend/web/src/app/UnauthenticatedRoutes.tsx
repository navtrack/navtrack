import { Navigate, Route, Routes } from "react-router-dom";
import { ForgotPasswordPage } from "../components/user/forgot-password/ForgotPasswordPage";
import { LoginPage } from "../components/user/login-page/LoginPage";
import { RegisterPage } from "../components/user/register-page/RegisterPage";
import { ResetPasswordPage } from "../components/user/reset-password-page/ResetPasswordPage";
import { Paths } from "./Paths";
import { ReactNode } from "react";

type UnauthenticatedRoutesProps = {
  children?: ReactNode;
};

export function UnauthenticatedRoutes(props: UnauthenticatedRoutesProps) {
  return (
    <Routes>
      <Route path={Paths.Register} element={<RegisterPage />} />
      <Route path={Paths.ForgotPassword} element={<ForgotPasswordPage />} />
      <Route path={Paths.ResetPassword} element={<ResetPasswordPage />} />
      <Route path={Paths.Home} element={<LoginPage />} index />
      {props.children}
      <Route path="*" element={<Navigate replace to={Paths.Home} />} />
    </Routes>
  );
}
