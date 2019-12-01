import React, { useState, useEffect } from "react";
import Notification from "./Notification";
import { NotificationService } from "./NotificationService";

type Props = {
    notification: Notification
}

export default function NotificationComponent(props: Props) {
    const [className, setClassName] = useState("fadeInDown animated bounce slow");

    useEffect(() => {
        console.log("starting transition in");
        props.notification.animating = true;

        setTimeout(() => {
            console.log("finished transition in");
            props.notification.animating = false;
        }, 2000);

        setTimeout(() => {
            console.log("starting transition out");
            setClassName("fadeOutUp animated bounce slow");
            props.notification.animating = true;
        }, 3000);

        setTimeout(() => {
            console.log("finished transition out");
            props.notification.animating = false;
            props.notification.show = false;
            NotificationService.cleanUp();
        }, 4500);
    }, [props.notification.animating, props.notification.show]);

    return (
        <div className={className}>
            <div className="col-md-4 offset-md-4">
                <div className="alert alert-default alert-dismissible  show m-4">
                    <span className="alert-icon"><i className="ni ni-like-2"></i></span>
                    <span className="alert-text">{props.notification.message}</span>
                    <button type="button" className="close" data-dismiss="alert" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
            </div>
        </div>
    );
}