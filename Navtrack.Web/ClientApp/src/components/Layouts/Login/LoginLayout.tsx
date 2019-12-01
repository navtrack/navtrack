import LoginNavbar from "./LoginNavbar";
import LoginFooter from "./LoginFooter";
import React, {ReactElement} from "react";

type Props = {
    children: ReactElement
}

export default function LoginLayout(props: Props) {
    return (
        <div className="main-content bg-default">
            <LoginNavbar/>
            {props.children}
            <LoginFooter/>
        </div>
    );
}