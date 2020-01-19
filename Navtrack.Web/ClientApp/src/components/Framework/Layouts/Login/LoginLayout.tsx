import React, { ReactNode } from "react";

type Props = {
    children: ReactNode
}

export default function LoginLayout(props: Props) {
    return (
        <div className="bg-default min-vh-100 d-flex">
            <div className="d-flex flex-column container flex-fill">
                {/* <LoginNavbar /> */}
                {props.children}
                {/* <LoginFooter /> */}
            </div>
        </div>
    );
}