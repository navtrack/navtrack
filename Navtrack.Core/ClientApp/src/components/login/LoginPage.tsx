import { Form, Formik } from "formik";
import React, { useEffect } from "react";
import { FormattedMessage, useIntl } from "react-intl";
import { useHistory } from "react-router";
import { Link } from "react-router-dom";
import { useLogin } from "../../services/hooks2/useLogin";
import Button from "../shared/elements/Button";
import TextInput from "../shared/forms/TextInput";
import LoginBox from "../shared/layouts/login/LoginBox";
import Icon from "../shared/util/Icon";
import { InitialLoginModel } from "./LoginModel";
import { GetLoginModelValidation } from "./LoginModelValidation";

export default function LoginPage() {
  const history = useHistory();
  const { login, isLoading, isSuccess, error } = useLogin();
  const intl = useIntl();

  useEffect(() => {
    if (isSuccess) {
      history.push("/");
    } else {
    }
  }, [history, isSuccess, login]);

  return (
    <LoginBox>
      <div className="text-center text-2xl font-medium mb-4">
        <FormattedMessage id="login.title" />
      </div>
      {error && error.message && (
        <div className="border bg-red-100 border-red-200 px-3 py-2 rounded mb-4 text-red-800">
          {error?.message}
        </div>
      )}
      <Formik
        initialValues={InitialLoginModel}
        onSubmit={(values) => login(values)}
        validationSchema={() => GetLoginModelValidation(intl)}>
        {({ values }) => (
          <Form>
            <TextInput name="email" title={"generic.email"} value={values.email} size="lg" />
            <TextInput
              name="password"
              title={"generic.password"}
              type="password"
              value={values.password}
              size="lg"
            />
            <Button
              type="submit"
              color="secondary"
              size="lg"
              disabled={isLoading}
              fullWidth
              className="mt-4">
              <Icon className="fa-spinner fa-spin mr-2" show={isLoading} />
              <FormattedMessage id="login.button" />
            </Button>
          </Form>
        )}
      </Formik>
      <div className="text-center text-xs mt-4">
        <Link to="/forgotpassword" className="ml-1 text-blue-600">
          <FormattedMessage id="login.forgot" />
        </Link>
      </div>
      <div className="text-center text-sm font-medium mt-4 border-t pt-4">
        <span className="text-gray-600">
          <FormattedMessage id="login.new.question" />
        </span>
        <Link to="/register" className="ml-1 text-blue-600">
          <FormattedMessage id="login.new.action" />
        </Link>
      </div>
    </LoginBox>
  );
}
