import { useRecoilState, useRecoilValue } from "recoil";
import { Snackbar } from "../snackbar/Snackbar";
import { notificationAtom, showNotificationAtom } from "./state";

export function Notification() {
  const notification = useRecoilValue(notificationAtom);
  const [showNotification, setShowNotification] =
    useRecoilState(showNotificationAtom);

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
