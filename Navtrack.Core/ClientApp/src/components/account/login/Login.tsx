import React, { FormEvent, useState } from "react";
import { useIntl, FormattedMessage } from "react-intl";
import { useHistory } from "react-router";
import { Link } from "react-router-dom";
import { AuthenticationService } from "../../../services/authentication/AuthenticationService";
import { ApiError } from "../../../services/httpClient/AppError";
import { useNewValidation } from "../../../services/validation/useValidationHook";
import { ValidateAction } from "../../../services/validation/ValidateAction";
import Button from "../../shared/elements/Button";
import TextInput from "../../shared/forms/TextInput";
import LoginBox from "../../shared/layouts/login/LoginBox";
import Icon from "../../shared/util/Icon";
import { LoginModel, DefaultLoginModel } from "./LoginModel";

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

const validateLogin: ValidateAction<LoginModel> = (object, validationResult, intl) => {
  if (!object.email) {
    validationResult.AddError("email", intl.formatMessage({ id: "login.email.required" }));
  }

  if (!object.password) {
    validationResult.AddError("password", intl.formatMessage({ id: "login.password.required" }));
  }
};
