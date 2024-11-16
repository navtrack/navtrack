import { useEffect } from "react";

export function useUnmount(callback: () => void) {
  useEffect(() => {
    return callback;
  }, [callback]);
}
