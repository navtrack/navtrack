import React, { FormEvent, useState, useContext } from "react";
import { Link, useHistory } from "react-router-dom";
import { AccountApi } from "../../services/Api/AccountApi";
import AppContext from "../../services/AppContext";
import LoginLayout from "components/Framework/Layouts/Login/LoginLayout";
import { ApiError, Errors } from "services/HttpClient/HttpClient";
import InputError, { AddError, HasErrors } from "components/Common/InputError";
import { LoginModel, DefaultLoginModel } from "./LoginModel"

export default function Login() {
    const [login, setLogin] = useState<LoginModel>(DefaultLoginModel);
    const [showLoadingIndicator, setShowLoadingIndicator] = useState(false);
    const [errors, setErrors] = useState<Errors>({});
    const history = useHistory();
    const { appContext, setAppContext } = useContext(AppContext);

    const signIn = async (e: FormEvent) => {
        e.preventDefault();

        const errors = validateModel(login);

        if (HasErrors(errors)) {
            setErrors(errors);
        } else {
            setShowLoadingIndicator(true);

            await AccountApi.login(login.email, login.password)
                .then(() => {
                    setAppContext({ ...appContext, user: { username: login.email }, authenticated: true })

                    history.push("/");
                })
                .catch((error: ApiError) => {
                    setErrors(error.errors);
                    setShowLoadingIndicator(false);
                });
        }
    };

    return (
        <LoginLayout>
            <div className="max-w-xs w-full flex flex-col items-center">
                <div className="h-16 m-3 ">
                    <a href="https://www.navtrack.io">
                        <img src="/navtrack.png" width="64" className="mb-4" alt="Navtrack" />
                    </a>
                </div>
                <form className="shadow-xl bg-white rounded px-8 w-full bg-gray-100" onSubmit={(e) => signIn(e)}>
                    <div className="text-center my-6">Sign in to Navtrack</div>
                    <div className="mb-4">
                        <input className="shadow appearance-none rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none" id="email" type="email" placeholder="Email"
                            value={login.email} onChange={(e) => setLogin({ ...login, email: e.target.value })}
                        />
                        <InputError name="email" errors={errors} />
                    </div>
                    <div className="mb-4">
                        <input className="shadow appearance-none rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:border focus:border-gray-900" id="password" type="password" placeholder="Password"
                            value={login.password} onChange={(e) => setLogin({ ...login, password: e.target.value })}
                        />
                        <InputError name="password" errors={errors} />
                    </div>
                    <div className="flex justify-center my-6">
                        <button className="shadow-md bg-gray-800 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded focus:outline-none" type="submit">
                            {showLoadingIndicator && <i className="fas fa-spinner fa-spin mr-2" />} Sign in
                    </button>
                    </div>
                </form>
                <div className="h-20 flex w-full">
                    <div className="flex-grow">
                        <Link to="/register" className="text-white text-xs">Create new account</Link>
                    </div>
                    <div className="flex-grow text-right">
                        {/* <Link to="/forgotpassword" className="text-white text-xs">Forgot password?</Link> */}
                    </div>
                </div>
            </div>
        </LoginLayout>
    );
}

const validateModel = (login: LoginModel): Record<string, string[]> => {
    const errors: Record<string, string[]> = {};

    if (login.email.length === 0) {
        AddError<LoginModel>(errors, "email", "Email is required.");
    }
    if (login.email.length === 0) {
        AddError<LoginModel>(errors, "password", "Password is required.");
    }

    return errors;
};