import React, {} from "react";
import {Errors} from "../../services/HttpClient/HttpClient";
import {getParameterCaseInsensitive} from "../../services/Util/ObjectUtil";

type Props = {
    name: string,
    errors: Errors
}

export default function InputError(props: Props) {
    const errors: string[] = getParameterCaseInsensitive(props.errors, props.name);
    return (
        <>
            {errors && errors.map((x, i) =>
            <p className="text-red-500 text-xs italic mt-2" key={i}>{x}</p>)}
        </>
    );
}

export const AddError = <T extends {}>(errors: Record<keyof T, string[]>, key: keyof T, message: string) => {
    if (key in errors) {
        errors[key].push(message);
    } else {
        errors[key] = [message];
    }
};

export const HasErrors = (errors: Record<string, string[]>): boolean => {
    return Object.keys(errors).length > 0;
};