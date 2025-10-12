import { atom } from "jotai";
import { Notification } from "./types";

export const notificationAtom = atom<Notification | undefined>(undefined);

export const showNotificationAtom = atom<boolean>(false);
