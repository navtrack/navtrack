import React from "react";
import {Link} from "react-router-dom";

export default function Login() {
    return (
        <>
            <div className="header bg-gradient-primary py-7 py-lg-8">
                <div className="container">
                    <div className="header-body text-center mb-1">
                        <div className="row justify-content-center">
                            <div className="col-lg-5 col-md-6">
                                <h1 className="text-white">Welcome!</h1>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="separator separator-bottom separator-skew zindex-100">
                    <svg x="0" y="0" viewBox="0 0 2560 100" preserveAspectRatio="none" version="1.1"
                         xmlns="http://www.w3.org/2000/svg">
                        <polygon className="fill-default" points="2560 0 2560 100 0 100"></polygon>
                    </svg>
                </div>
            </div>
            <div className="container mt--8 pb-5">
                <div className="row justify-content-center">
                    <div className="col-lg-5 col-md-7">
                        <div className="card bg-secondary shadow border-0">
                            <div className="card-body px-lg-5 py-lg-5">
                                <div className="text-center text-muted mb-4">
                                    <small>Sign in to Navtrack.</small>
                                </div>
                                <form>
                                    <div className="form-group mb-3">
                                        <div className="input-group input-group-alternative">
                                            <div className="input-group-prepend">
                                                <span className="input-group-text"><i
                                                    className="ni ni-email-83"/></span>
                                            </div>
                                            <input className="form-control" placeholder="Email" type="email"/>
                                        </div>
                                    </div>
                                    <div className="form-group">
                                        <div className="input-group input-group-alternative">
                                            <div className="input-group-prepend">
                                                <span className="input-group-text"><i
                                                    className="ni ni-lock-circle-open"/></span>
                                            </div>
                                            <input className="form-control" placeholder="Password" type="password"/>
                                        </div>
                                    </div>
                                    <div className="custom-control custom-control-alternative custom-checkbox">
                                        <input className="custom-control-input" id=" customCheckLogin" type="checkbox"/>
                                        <label className="custom-control-label" htmlFor=" customCheckLogin">
                                            <span className="text-muted">Remember me</span>
                                        </label>
                                    </div>
                                    <div className="text-center">
                                        <button type="button" className="btn btn-primary mt-4">Sign in</button>
                                    </div>
                                </form>
                            </div>
                        </div>
                        <div className="row mt-3">
                            <div className="col-6">
                                <Link to="/SignUp" className="text-white"><small>Create new account</small></Link>
                            </div>
                            <div className="col-6 text-right">
                                <Link to="/ForgotPassword" className="text-white"><small>Forgot password?</small></Link>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}