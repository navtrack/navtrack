import { atom } from "recoil";
import { Notification } from "./types";

export const notificationAtom = atom<Notification | undefined>({
  key: "Notification",
  default: undefined
});

export const showNotificationAtom = atom<boolean>({
  key: "Notification:Show",
  default: false
});
