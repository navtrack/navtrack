import { useState } from "react";
import { Notification } from "./Notification";
import { useNotification } from "./useNotification";

export default {
  Default: () => {
    const { showNotification } = useNotification();
    const [count, setCount] = useState(1);

    return (
      <>
        <button
          onClick={() => {
            showNotification({
              type: "success",
              description: `Asset ${count} added successfully!`
            });
            setCount((p) => ++p);
          }}>
          Show notification
        </button>
        <Notification />
      </>
    );
  }
};
