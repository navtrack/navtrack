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
        .email("generic.email.invalid")
        .required("generic.email.required"),
    password: string().required("generic.password.required")
}).defined();