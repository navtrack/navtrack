import { useCallback, useState } from "react";
import { useRecoilState, useSetRecoilState } from "recoil";
import { notificationAtom, showNotificationAtom } from "./state";
import { Notification } from "./types";

export default function useNotification() {
  const setNotification = useSetRecoilState(notificationAtom);
  const [showNotificationState, setShowNotification] = useRecoilState(showNotificationAtom);
  const [timeout, setLocalTimeout] = useState<NodeJS.Timeout | undefined>(undefined);

  const displayNotification = useCallback(
    (notification: Notification) => {
      setNotification(notification);
      setShowNotification(true);
      if (timeout) {
        clearTimeout(timeout);
      }
      setLocalTimeout(
        setTimeout(() => {
          setShowNotification(false);
        }, 5000)
      );
    },
    [setNotification, setShowNotification, timeout]
  );

  const showNotification = useCallback(
    (notification: Notification) => {
      if (showNotificationState) {
        setShowNotification(false);
        setTimeout(() => {
          displayNotification(notification);
        }, 300);
      } else {
        displayNotification(notification);
      }
    },
    [displayNotification, setShowNotification, showNotificationState]
  );

  return { showNotification };
}
