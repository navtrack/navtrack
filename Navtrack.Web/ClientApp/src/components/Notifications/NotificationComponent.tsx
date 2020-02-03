import React, { useState, useEffect } from "react";
import Notification from "./Notification";
import { NotificationService } from "./NotificationService";
import classNames from "classnames";
import Icon from "components/Framework/Util/Icon";
import { NotificationType } from "./NotificationType";

type Props = {
  notification: Notification
}

export default function NotificationComponent(props: Props) {
  const [className, setClassName] = useState("fadeInDown animated bounce fast");
  const [show, setShow] = useState(true);

  let background = props.notification.type === NotificationType.Info ? "bg-gray-200" : "bg-red-200";
  let textColor = props.notification.type === NotificationType.Info ? "text-gray-700" : "text-red-700";

  useEffect(() => {
    props.notification.animating = true;

    setTimeout(() => {
      props.notification.animating = false;
    }, 2000);

    setTimeout(() => {
      setClassName("fadeOutUp animated bounce slow");
      props.notification.animating = true;
    }, 3000);

    setTimeout(() => {
      props.notification.animating = false;
      props.notification.visible = false;
      NotificationService.cleanUp();
    }, 4500);
  }, [props.notification.animating, props.notification.visible]);

  return (
    <>
      {show &&
        <div className={classNames(className, "w-1/3")}>
          <div className={classNames("bg-gray-300 mt-4 rounded shadow-lg p-4 pl-4 flex w-full ", background)}>
            <div className={classNames("flex-grow", textColor)}>
              {props.notification.message}
            </div>
            <div>
              <button type="button" className="focus:outline-none" onClick={() => setShow(false)}>
                <Icon className={classNames("fa-times", textColor)} />
              </button>
            </div>
          </div>
        </div>
      }
    </>
  );
}