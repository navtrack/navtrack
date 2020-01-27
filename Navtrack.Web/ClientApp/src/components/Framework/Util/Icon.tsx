import React from "react";

type Props = {
    icon: string
}

export default function Icon(props: Props) {
    return (<i className={`fas ${props.icon} mr-2`} />);
}