import React, { useState, useEffect } from "react";
import { UserModel, DefaultUserModel } from "services/Api/Model/UserModel";
import { Errors, ApiError } from "services/HttpClient/HttpClient";
import { useHistory } from "react-router";
import { UserApi } from "services/Api/UserApi";
import InputError, { HasErrors, AddError } from "components/Common/InputError";
import { addNotification } from "components/Notifications";
import AdminLayout from "components/Framework/Layouts/Admin/AdminLayout";

type Props = {
  id?: number
}

export default function UserEdit(props: Props) {
  const [user, setUser] = useState<UserModel>(DefaultUserModel);
  const [errors, setErrors] = useState<Errors>({});
  const [show, setShow] = useState(!props.id);
  const history = useHistory();

  useEffect(() => {
    if (props.id) {
      UserApi.get(props.id).then(x => {
        setUser(x);
        setShow(true);
      });
    }
  }, [props.id]);

  const submitForm = async () => {
    const errors = validateModel(user);

    if (HasErrors(errors)) {
      setErrors(errors);
    } else {
      if (user.id > 0) {
        UserApi.update(user)
          .then(() => {
            history.push("/users");
            addNotification("User saved successfully.");
          })
          .catch((error: ApiError) => {
            setErrors(error.errors)
          });
      } else {
        UserApi.add(user)
          .then(() => {
            history.push("/users");
            addNotification("User added successfully.");
          })
          .catch((error: ApiError) => {
            setErrors(error.errors);
          });
      }
    }
  };

  return (
    <AdminLayout>
      {show &&
        <div className="shadow rounded bg-white flex flex-col">
          <div className="p-5">
            <div className="font-medium text-lg">{props.id ? <>Edit user</> : <>Add user</>}</div>
          </div>
          <div className="p-5">
            <div className="flex flex-row mb-5">
              <div className="w-20 text-gray-700 font-medium h-10 flex items-center">Email</div>
              <div className="text-gray-700 font-medium w-5/12">
                <input className="h-10 shadow bg-gray-100 appearance-none rounded py-2 px-3 text-gray-700 focus:outline-none focus:border focus:border-gray-900 w-full"
                  value={user.email}
                  placeholder="Email"
                  onChange={(e) => {
                    setUser({ ...user, email: e.target.value });
                    setErrors({ ...errors, imei: [] });
                  }} />
                <InputError name="email" errors={errors} />
              </div>
            </div>
          </div>
          <div className="p-5">
            <button className="shadow-md bg-gray-200 hover:bg-gray-300 py-2 px-4 rounded focus:outline-none"
              onClick={() => history.goBack()}>Cancel</button>
            <button className="shadow-md bg-gray-800 hover:bg-gray-900 text-white py-2 px-4 rounded focus:outline-none ml-3"
              onClick={submitForm}>Save</button>
          </div>
        </div>}
    </AdminLayout>
  );
}

const validateModel = (user: UserModel): Record<string, string[]> => {
  const errors: Record<string, string[]> = {};

  if (user.email.length === 0) {
    AddError(errors, "email", "The Email field is required.");
  }

  return errors;
};