import { useAtom, useAtomValue } from "jotai";
import { Snackbar } from "../snackbar/Snackbar";
import { notificationAtom, showNotificationAtom } from "./state";

export function Notification() {
  const notification = useAtomValue(notificationAtom);
  const [showNotification, setShowNotification] =
    useAtom(showNotificationAtom);

  return (
    <Snackbar
      show={showNotification}
      type={notification?.type}
      title={notification?.title}
      description={notification?.description}
      onCloseClick={() => setShowNotification(false)}
    />
  );
}
