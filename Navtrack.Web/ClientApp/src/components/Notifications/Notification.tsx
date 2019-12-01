import {NotificationType} from "./NotificationType";

export default class Notification {
    message: string;
    show: boolean;
    id: number;
    animating: boolean;
    type: NotificationType;

    constructor(message: string, type: NotificationType) {
        this.message = message;
        this.animating = false;
        this.show = true;
        this.id = new Date().getTime();
        this.type = type;
    }
}