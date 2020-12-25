import { Formik, Form } from "formik";
import React from "react";
import { useIntl, FormattedMessage } from "react-intl";
import { Link } from "react-router-dom";
import { useAccountRegisterMutation } from "../../services/mutations/useAccountRegisterMutation";
import Button from "../shared/elements/Button";
import TextInput from "../shared/forms/TextInput";
import LoginBox from "../shared/layouts/login/LoginBox";
import Icon from "../shared/util/Icon";
import { InitialRegisterModel } from "./RegisterModel";
import { GetRegisterModelValidation } from "./RegisterModelValidation";

export default function Register() {
  const intl = useIntl();
  const register = useAccountRegisterMutation();

  return (
    <LoginBox>
      {register.isSuccess ? (
        <>
          <div className="text-center">
            <FormattedMessage id="register.success" />
          </div>
          <div className="text-center mt-4">
            <Link to="/login" className="ml-1 text-blue-600">
              <FormattedMessage id="register.continue" />
            </Link>
          </div>
        </>
      ) : (
        <>
          <div className="text-center text-2xl font-medium mb-4">
            <FormattedMessage id="register.title" />
          </div>
          {register.error && register.error.message && (
            <div className="border bg-red-100 border-red-200 px-3 py-2 rounded mb-4 text-red-800">
              {register.error?.message}
            </div>
          )}
          <Formik
            initialValues={InitialRegisterModel}
            onSubmit={(values) => register.mutate(values)}
            validationSchema={() => GetRegisterModelValidation(intl)}
            initialErrors={register.error?.fields}
            enableReinitialize>
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
                <TextInput
                  name="confirmPassword"
                  title={"generic.confirmPassword"}
                  type="password"
                  value={values.confirmPassword}
                  size="lg"
                />
                <Button
                  type="submit"
                  color="secondary"
                  size="lg"
                  disabled={register.isLoading}
                  fullWidth
                  className="mt-4">
                  <Icon className="fa-spinner fa-spin mr-2" show={register.isLoading} />
                  <FormattedMessage id="register.button" />
                </Button>
              </Form>
            )}
          </Formik>
          <div className="text-center text-sm font-medium mt-4">
            <span className="text-gray-600">
              <FormattedMessage id="register.existing.question" />
            </span>
            <Link to="/login" className="ml-1 text-blue-600">
              <FormattedMessage id="register.existing.action" />
            </Link>
          </div>
        </>
      )}
    </LoginBox>
  );
}
