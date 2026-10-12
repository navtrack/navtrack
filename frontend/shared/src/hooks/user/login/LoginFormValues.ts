import {object, ObjectSchema, string} from "yup";

export type LoginFormValues = {
    email: string;
    password: string;
};

export const InitialLoginFormValues: LoginFormValues = {
    email: "",
    password: ""
};

export const loginFormValuesValidationSchema: ObjectSchema<LoginFormValues> = object({
    email: string()
        .email("email.invalid")
        .required("email.required"),
    password: string().required("password.required")
}).defined();