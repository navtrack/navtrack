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
                    setAppContext({ ...appContext, user: { username: login.email, authenticated: true } })

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
            <div className="flex-grow-1 d-flex align-items-center flex-column justify-content-center">
                <a href="https://www.navtrack.io">
                    <img src="/navtrack.png" width="64" className="mb-4" alt="Navtrack" />
                </a>
                <div className="login">
                    <div className="card bg-secondary shadow border-0 login">
                        <div className="card-body">
                            <div className="text-center mb-4 text-default">Sign in to Navtrack</div>
                            <form onSubmit={(e) => signIn(e)}>
                                <div className="form-group mb-3">
                                    <div className="input-group input-group-alternative">
                                        <div className="input-group-prepend">
                                            <span className="input-group-text">
                                                <i className="fas fa-envelope" />
                                            </span>
                                        </div>
                                        <input className="form-control" placeholder="Email" type="email"
                                            value={login.email} onChange={(e) => setLogin({ ...login, email: e.target.value })} />
                                    </div>
                                    <InputError name="email" errors={errors} />
                                </div>
                                <div className="form-group mb-4">
                                    <div className="input-group input-group-alternative">
                                        <div className="input-group-prepend">
                                            <span className="input-group-text">
                                                <i className="fas fa-unlock-alt" />
                                            </span>
                                        </div>
                                        <input className="form-control" placeholder="Password" type="password"
                                            value={login.password} onChange={(e) => setLogin({ ...login, password: e.target.value })} />
                                    </div>
                                    <InputError name="password" errors={errors} />
                                </div>
                                {/* <div className="custom-control custom-control-alternative custom-checkbox">
                                    <input className="custom-control-input" id=" customCheckLogin" type="checkbox" />
                                    <label className="custom-control-label" htmlFor=" customCheckLogin">
                                        <span className="text-muted">Remember me</span>
                                    </label>
                                </div> */}
                                <div className="text-center">
                                    <button type="submit" className="btn btn-primary">
                                        {showLoadingIndicator && <i className="fas fa-spinner fa-spin mr-2" />}
                                        Sign in
                                    </button>
                                </div>
                            </form>
                        </div>
                    </div>
                    <div className="d-flex">
                        <div className="flex-fill">
                            <Link to="/SignUp" className="text-white"><small>Create new account</small></Link>
                        </div>
                        <div className="flex-fill text-right">
                            <Link to="/ForgotPassword" className="text-white"><small>Forgot password?</small></Link>
                        </div>
                    </div>
                </div>
                <div className="login-hack"></div>
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