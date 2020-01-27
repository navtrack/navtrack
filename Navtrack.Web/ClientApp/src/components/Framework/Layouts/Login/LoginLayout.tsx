import React, { ReactNode } from "react";

type Props = {
    children: ReactNode
}

export default function LoginLayout(props: Props) {
    return (
        <div className="bg-gray-800 flex min-h-screen items-center justify-center flex-col">
            {props.children}
        </div>
    );
}