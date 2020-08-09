import React, { FormEvent, useState } from "react";
import { Link } from "react-router-dom";
import { ApiError } from "framework/httpClient/AppError";
import { RegisterModel, DefaultRegisterModel } from "./RegisterModel";
import { AccountApi } from "apis/AccountApi";
import { Validator } from "framework/validation/Validator";
import { useNewValidation } from "framework/validation/useValidationHook";
import { FormattedMessage, useIntl } from "react-intl";
import TextInput from "components/library/forms/TextInput";
import Button from "components/library/elements/Button";
import Icon from "components/library/util/Icon";
import LoginBox from "components/framework/layouts/login/LoginBox";

export default function Register() {
  const [registerModel, setRegisterModel] = useState<RegisterModel>(DefaultRegisterModel);
  const [validate, validationResult, setApiError] = useNewValidation(validateLogin);
  const [showLoadingIndicator, setShowLoadingIndicator] = useState(false);
  const [showSuccess, setShowSuccess] = useState(false);
  const intl = useIntl();

  const signIn = async (e: FormEvent) => {
    e.preventDefault();

    if (validate(registerModel)) {
      setShowLoadingIndicator(true);

      AccountApi.register(registerModel)
        .then(() => {
          setShowSuccess(true);
        })
        .catch((error: ApiError<RegisterModel>) => {
          setApiError(error);
          setShowLoadingIndicator(false);
        });
    }
  };

  return (
    <LoginBox
      links={
        <>
          <div className="flex-grow">
            <Link to="/login" className="text-white text-xs">
              <FormattedMessage id="register.backToLogin" />
            </Link>
          </div>
          <div className="flex-grow text-right">
            {/* <Link to="/forgotpassword" className="text-white text-xs">Forgot password?</Link> */}
          </div>
        </>
      }>
      {showSuccess ? (
        <div className="text-center my-6">
          Your account was successfully created, you can login now.
        </div>
      ) : (
        <>
          <div className="text-center my-6 text-lg font-semibold">
            <FormattedMessage id="register.title" />
          </div>
          <form onSubmit={(e) => signIn(e)}>
            <div className="mb-4">
              <TextInput
                name={intl.formatMessage({ id: "register.email" })}
                value={registerModel.email}
                validationResult={validationResult.property.email}
                className="mb-3"
                onChange={(e) => setRegisterModel({ ...registerModel, email: e.target.value })}
              />
              <TextInput
                name={intl.formatMessage({ id: "register.password" })}
                type="password"
                value={registerModel.password}
                validationResult={validationResult.property.password}
                className="mb-3"
                onChange={(e) => setRegisterModel({ ...registerModel, password: e.target.value })}
              />
              <TextInput
                name={intl.formatMessage({ id: "register.confirmPassword" })}
                type="password"
                value={registerModel.confirmPassword}
                validationResult={validationResult.property.confirmPassword}
                className="mb-3"
                onChange={(e) =>
                  setRegisterModel({ ...registerModel, confirmPassword: e.target.value })
                }
              />
            </div>
            <div className="flex justify-center my-6">
              <Button color="secondary" size="sm" disabled={validationResult.HasErrors()}>
                <Icon className="fa-spinner fa-spin mr-2" show={showLoadingIndicator} />
                <FormattedMessage id="register.button" />
              </Button>
            </div>
          </form>
        </>
      )}
    </LoginBox>
  );
}

const validateLogin: Validator<RegisterModel> = (model, validationResult, intl) => {
  if (!model.email) {
    validationResult.AddError("email", intl.formatMessage({ id: "register.email.required" }));
  }
  if (model.password.length === 0) {
    validationResult.AddError("password", intl.formatMessage({ id: "register.password.required" }));
  }
  if (model.confirmPassword.length === 0) {
    validationResult.AddError(
      "confirmPassword",
      intl.formatMessage({ id: "register.confirmPassword.required" })
    );
  } else if (model.password !== model.confirmPassword) {
    validationResult.AddError(
      "confirmPassword",
      intl.formatMessage({ id: "register.confirmPassword.match" })
    );
  }
};
