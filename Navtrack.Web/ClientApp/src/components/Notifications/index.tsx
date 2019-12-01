import React from "react";
import { useState, useEffect } from "react";
import Notification from "./Notification";
import { NotificationService } from "./NotificationService";
import NotificationComponent from "./NotificationComponent";
import 'animate.css/animate.css'
import { NotificationType } from "./NotificationType";

export const addNotification = (message: string, type: NotificationType = NotificationType.Info) => {
    NotificationService.addNotification(message, type);
}

export default function Notifications() {
    const [notifications, setNotifications] = useState<Notification[]>([]);

    useEffect(() => {
        if (!NotificationService.setNotificationsInState) {
            NotificationService.setNotificationsInState = setNotifications;
        }
    }, []);

    return (
        <div className="fixed-top">
            {notifications.map((x, i) => <NotificationComponent notification={x} key={x.id} />)}
        </div>
    )
}