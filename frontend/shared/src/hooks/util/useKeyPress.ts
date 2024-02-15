import { useCallback, useEffect } from "react";

export function useKeyPress(code: string, callback?: () => void) {
  const handleKeyPress = useCallback(
    (e: KeyboardEvent) => {
      if (e.code === code && callback !== undefined) {
        e.preventDefault();
        callback();
      }
    },
    [callback, code]
  );

  useEffect(() => {
    if (callback !== undefined) {
      window.addEventListener("keydown", handleKeyPress);
      return () => {
        window.removeEventListener("keydown", handleKeyPress);
      };
    }
  }, [callback, handleKeyPress]);
}
