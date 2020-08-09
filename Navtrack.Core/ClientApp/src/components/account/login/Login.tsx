import React, { FormEvent, useState } from "react";
import { LoginModel, DefaultLoginModel } from "./LoginModel";
import { ApiError } from "framework/httpClient/AppError";
import { useHistory } from "react-router";
import { AuthenticationService } from "framework/authentication/AuthenticationService";
import { useNewValidation } from "framework/validation/useValidationHook";
import { useIntl, FormattedMessage } from "react-intl";
import { Link } from "react-router-dom";
import TextInput from "components/library/forms/TextInput";
import Button from "components/library/elements/Button";
import Icon from "components/library/util/Icon";
import { Validator } from "framework/validation/Validator";
import LoginBox from "components/framework/layouts/login/LoginBox";

export default function Login() {
  const [login, setLogin] = useState<LoginModel>(DefaultLoginModel);
  const [validate, validationResult, setApiError] = useNewValidation(validateLogin);
  const [showLoadingIndicator, setShowLoadingIndicator] = useState(false);
  const history = useHistory();
  const intl = useIntl();

  const signIn = async (e: FormEvent) => {
    e.preventDefault();

    if (validate(login)) {
      setShowLoadingIndicator(true);

      AuthenticationService.login(login.email, login.password)
        .then(() => {
          history.push("/");
        })
        .catch((error: ApiError<LoginModel>) => {
          setApiError(error);
          setShowLoadingIndicator(false);
        });
    }
  };

  return (
    <LoginBox
      links={
        <>
          <div>
            <Link to="/register" className="text-white text-xs">
              <FormattedMessage id="login.createAccount" />
            </Link>
          </div>
          {/* <div className="flex-grow text-right">
            <Link to="/forgotpassword" className="text-white text-xs">
              <FormattedMessage id="login.forgotPassword" />
            </Link>
          </div> */}
        </>
      }>
      <div className="text-center my-6">
        <FormattedMessage id="login.title" />
      </div>
      <form onSubmit={(e) => signIn(e)}>
        <div className="mb-4">
          <TextInput
            name={intl.formatMessage({ id: "login.email" })}
            value={login.email}
            validationResult={validationResult.property.email}
            className="mb-3"
            onChange={(e) => setLogin({ ...login, email: e.target.value })}
          />
          <TextInput
            name={intl.formatMessage({ id: "login.password" })}
            type="password"
            value={login.password}
            validationResult={validationResult.property.password}
            className="mb-3"
            onChange={(e) => setLogin({ ...login, password: e.target.value })}
          />
        </div>
        <div className="flex justify-center my-6">
          <Button color="secondary" size="sm" disabled={validationResult.HasErrors()}>
            <Icon className="fa-spinner fa-spin mr-2" show={showLoadingIndicator} />
            <FormattedMessage id="login.button" />
          </Button>
        </div>
      </form>
    </LoginBox>
  );
}

const validateLogin: Validator<LoginModel> = (object, validationResult, intl) => {
  if (!object.email) {
    validationResult.AddError("email", intl.formatMessage({ id: "login.email.required" }));
  }

  if (!object.password) {
    validationResult.AddError("password", intl.formatMessage({ id: "login.password.required" }));
  }
};
