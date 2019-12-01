import Notification from "./Notification";
import { NotificationType } from "./NotificationType";

export const NotificationService: {
    notifications: Notification[],
    setNotificationsInState: ((param: Notification[]) => void) | null,
    addNotification: (message: string, type: NotificationType) => void,
    animationInProgress: () => boolean,
    cleanUp: () => void,
} = {
    notifications: [],

    addNotification: (message: string, type: NotificationType = NotificationType.Info) => {
        let notifications: Notification[] = GetNotificationsArray();
        let notification = new Notification(message, type)

        addNotificationToArray(notifications, notification);
        updateNotificationsState(notifications);
    },

    setNotificationsInState: null,

    animationInProgress: () => NotificationService.notifications.filter(x => x.animating).length > 0,

    cleanUp: () => {
        if (NotificationService.notifications.filter(x => x.show || x.animating).length === 0) {
            updateNotificationsState([]);
        }
    }
}

function GetNotificationsArray() {
    let visibleNotifications: number = NotificationService.notifications.filter(x => x.animating || x.show).length;

    let newNotifications: Notification[] = visibleNotifications > 0 ? NotificationService.notifications.slice() : [];

    return newNotifications;
}

function addNotificationToArray(newNotifications: Notification[], notification: Notification) {
    let added = false;

    for (let i = 0; i < newNotifications.length && !added; i++) {
        if (!newNotifications[i].show) {
            newNotifications[i] = notification;
            added = true;
        }
    }
   
    if (!added) {
        newNotifications.push(notification);
    }
}

function updateNotificationsState(newNotifications: Notification[]) {
    NotificationService.notifications = newNotifications;

    if (NotificationService.setNotificationsInState) {
        NotificationService.setNotificationsInState(NotificationService.notifications);
    }
}