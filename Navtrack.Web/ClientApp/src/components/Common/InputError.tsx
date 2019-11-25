import React, { } from "react";

type Props = {
    name: string,
    errors: { [id: string] : string[]; }
}

export default function InputError(props: Props) {
    const hasErrors = props.errors[props.name];

    return (
        <>
            {hasErrors && props.errors[props.name].map((x, i) =>
            <div className="text-red text-sm mt-1" key={i}>{props.errors[props.name][i]}</div>)}
        </>
    );
}