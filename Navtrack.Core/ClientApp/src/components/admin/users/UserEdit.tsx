import React, { useState, useEffect } from "react";
import { useIntl } from "react-intl";
import { useHistory } from "react-router";
import { UserApi } from "../../../apis/UserApi";
import { useNewValidation } from "../../../services/validation/useValidationHook";
import { ValidateAction } from "../../../services/validation/ValidateAction";
import Button from "../../shared/elements/Button";
import Form from "../../shared/forms/Form";
import TextInput from "../../shared/forms/TextInput";
import { addNotification } from "../../shared/notifications/Notifications";

type Props = {
  id?: number;
};

export default function UserEdit(props: Props) {
  // const [user, setUser] = useState<UserModel>(DefaultUserModel);
  // const [validate, validationResult, setApiError] = useNewValidation(validateUser);
  const [show, setShow] = useState(!props.id);
  const history = useHistory();
  const intl = useIntl();

  // useEffect(() => {
  //   if (props.id) {
  //     UserApi.get(props.id).then((x) => {
  //       setUser(x);
  //       setShow(true);
  //     });
  //   }
  // }, [props.id]);

  // const handleSubmit = async () => {
  //   if (validate(user)) {
  //     if (user.id > 0) {
  //       UserApi.update(user)
  //         .then(() => {
  //           history.push("/admin/users");
  //           addNotification(intl.formatMessage({ id: "admin.user.edit.notification" }));
  //         })
  //         .catch(setApiError);
  //     } else {
  //       UserApi.add(user)
  //         .then(() => {
  //           history.push("/admin/users");
  //           addNotification(intl.formatMessage({ id: "admin.user.add.notification" }));
  //         })
  //         .catch(setApiError);
  //     }
  //   }
  // };

  return (
    <>
      {/* {show && (
        <Form
          title={intl.formatMessage({
            id: props.id ? "admin.user.edit.title" : "admin.user.add.title"
          })}
          actions={
            <Button color="primary" onClick={handleSubmit} disabled={validationResult.HasErrors()}>
              {intl.formatMessage({
                id: props.id ? "admin.user.edit.action" : "admin.user.add.action"
              })}
            </Button>
          }>
          <TextInput
            name={intl.formatMessage({ id: "admin.user.edit.email" })}
            value={user.email}
            validationResult={validationResult.property.email}
            className="mb-3"
            onChange={(e) => {
              setUser({ ...user, email: e.target.value });
            }}
          />
          <TextInput
            name={intl.formatMessage({ id: "admin.user.edit.password" })}
            value={user.password}
            validationResult={validationResult.property.password}
            className="mb-3"
            type="password"
            onChange={(e) => {
              setUser({ ...user, password: e.target.value });
            }}
          />
          <TextInput
            name={intl.formatMessage({ id: "admin.user.edit.confirmPassword" })}
            value={user.confirmPassword}
            validationResult={validationResult.property.confirmPassword}
            className="mb-3"
            type="password"
            onChange={(e) => {
              setUser({ ...user, confirmPassword: e.target.value });
            }}
          />
        </Form>
      )} */}
    </>
  );
}
// const validateUser: ValidateAction<UserModel> = (model, validationResult, intl) => {
//   if (!model.email) {
//     validationResult.AddError(
//       "email",
//       intl.formatMessage({ id: "admin.user.edit.email.required" })
//     );
//   }
//   if (model.id <= 0 && !model.password) {
//     validationResult.AddError(
//       "password",
//       intl.formatMessage({ id: "admin.user.add.password.required" })
//     );
//   }
//   if (model.id <= 0 && !model.confirmPassword) {
//     validationResult.AddError(
//       "confirmPassword",
//       intl.formatMessage({ id: "admin.user.add.confirmPassword.required" })
//     );
//   } else if (
//     (model.password || model.confirmPassword) &&
//     model.password !== model.confirmPassword
//   ) {
//     validationResult.AddError(
//       "confirmPassword",
//       intl.formatMessage({ id: "admin.user.edit.confirmPassword.match" })
//     );
//   }
// };
