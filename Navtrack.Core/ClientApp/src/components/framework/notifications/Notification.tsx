import {NotificationType} from "./NotificationType";

export default class Notification {
    message: string;
    visible: boolean;
    id: number;
    animating: boolean;
    type: NotificationType;

    constructor(message: string, type: NotificationType) {
        this.message = message;
        this.animating = false;
        this.visible = true;
        this.id = new Date().getTime();
        this.type = type;
    }
}