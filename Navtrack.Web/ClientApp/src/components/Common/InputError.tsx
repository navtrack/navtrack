import React, {} from "react";
import {Errors} from "../../services/HttpClient/HttpClient";
import {getParameterCaseInsensitive} from "../../services/ObjectUtil";

type Props = {
    name: string,
    errors: Errors
}

export default function InputError(props: Props) {
    const errors: string[] = getParameterCaseInsensitive(props.errors, props.name);
    return (
        <>
            {errors && errors.map((x, i) =>
                <div className="text-red text-sm mt-1" key={i}>{x}</div>)}
        </>
    );
}

export const AddError = (errors: Record<string, string[]>, key: string, message: string) => {
    if (key in errors) {
        errors[key].push(message);
    } else {
        errors[key] = [message];
    }
};

export const HasErrors = (errors: Record<string, string[]>): boolean => {
    return Object.keys(errors).length > 0;
};