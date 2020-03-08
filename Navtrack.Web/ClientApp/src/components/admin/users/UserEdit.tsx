import React, { useState, useEffect } from "react";
import { UserModel, DefaultUserModel } from "services/api/types/user/UserModel";
import { AppError } from "services/httpClient/AppError";
import { useHistory } from "react-router";
import { UserApi } from "services/api/UserApi";
import InputError, { HasErrors, ClearError, AddError } from "components/common/InputError";
import { addNotification } from "components/framework/notifications/Notifications";
import AdminLayout from "components/framework/layouts/admin/AdminLayout";
import { ValidationResult } from "components/common/ValidatonResult";

type Props = {
  id?: number;
};

export default function UserEdit(props: Props) {
  const [user, setUser] = useState<UserModel>(DefaultUserModel);
  const [error, setError] = useState<AppError>();
  const [show, setShow] = useState(!props.id);
  const history = useHistory();

  useEffect(() => {
    if (props.id) {
      UserApi.get(props.id)
        .then(x => {
          setUser(x);
          setShow(true);
        })
        .catch(setError);
    }
  }, [props.id]);

  const submitForm = async () => {
    const errors = validateModel(user);

    if (HasErrors(errors)) {
      setError(new AppError(errors));
    } else {
      if (user.id > 0) {
        UserApi.update(user)
          .then(() => {
            history.push("/admin/users");
            addNotification("User saved successfully.");
          })
          .catch(setError);
      } else {
        UserApi.add(user)
          .then(() => {
            history.push("/admin/users");
            addNotification("User added successfully.");
          })
          .catch(setError);
      }
    }
  };

  return (
    <AdminLayout>
      {show && (
        <div className="shadow rounded bg-white flex flex-col">
          <div className="p-3">
            <div className="font-medium text-lg">{props.id ? <>Edit user</> : <>Add user</>}</div>
          </div>
          <div className="p-3">
            <div className="flex flex-row mb-5">
              <div className="w-40 text-gray-700 font-medium h-10 flex items-center">Email</div>
              <div className="text-gray-700 font-medium w-5/12">
                <input
                  className="h-10 shadow bg-gray-100 appearance-none rounded py-2 px-3 text-gray-700 focus:outline-none focus:border focus:border-gray-900 w-full"
                  value={user.email}
                  placeholder="Email"
                  onChange={e => {
                    setUser({ ...user, email: e.target.value });
                    setError(x => ClearError<UserModel>(x, "email"));
                  }}
                />
                <InputError name="email" error={error} />
              </div>
            </div>
            <div className="flex flex-row mb-5">
              <div className="w-40 text-gray-700 font-medium h-10 flex items-center">Password</div>
              <div className="text-gray-700 font-medium w-5/12">
                <input
                  className="h-10 shadow bg-gray-100 appearance-none rounded py-2 px-3 text-gray-700 focus:outline-none focus:border focus:border-gray-900 w-full"
                  value={user.password}
                  placeholder="Password"
                  type="password"
                  onChange={e => {
                    setUser({ ...user, password: e.target.value });
                    setError(x => ClearError<UserModel>(x, "password"));
                  }}
                />
                <InputError name="password" error={error} />
              </div>
            </div>
            <div className="flex flex-row mb-5">
              <div className="w-40 text-gray-700 font-medium h-10 flex items-center">
                Confirm password
              </div>
              <div className="text-gray-700 font-medium w-5/12">
                <input
                  className="h-10 shadow bg-gray-100 appearance-none rounded py-2 px-3 text-gray-700 focus:outline-none focus:border focus:border-gray-900 w-full"
                  value={user.confirmPassword}
                  placeholder="Confirm Password"
                  type="password"
                  onChange={e => {
                    setUser({ ...user, confirmPassword: e.target.value });
                    setError(x => ClearError<UserModel>(x, "confirmPassword"));
                  }}
                />
                <InputError name="confirmPassword" error={error} />
              </div>
            </div>
          </div>
          <div className="p-3">
            <button
              className="shadow-md bg-gray-200 hover:bg-gray-300 py-2 px-4 rounded focus:outline-none"
              onClick={() => history.goBack()}>
              Cancel
            </button>
            <button
              className="shadow-md bg-gray-800 hover:bg-gray-900 text-white py-2 px-4 rounded focus:outline-none ml-3"
              onClick={submitForm}>
              Save
            </button>
          </div>
        </div>
      )}
    </AdminLayout>
  );
}

const validateModel = (user: UserModel): ValidationResult => {
  const validationResult: ValidationResult = {};

  if (user.email.length === 0) {
    AddError<UserModel>(validationResult, "email", "The email is required.");
  }
  if (
    user.confirmPassword &&
    user.confirmPassword.length > 0 &&
    user.password !== user.confirmPassword
  ) {
    AddError<UserModel>(validationResult, "confirmPassword", "The passwords must match.");
  }

  return validationResult;
};
