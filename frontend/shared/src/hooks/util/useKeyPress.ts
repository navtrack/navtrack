import { useCallback, useEffect } from "react";

export function useKeyPress(code: string, callback: () => void) {
  const handleKeyPress = useCallback(
    (e: KeyboardEvent) => {
      if (e.code === code) {
        e.preventDefault();
        callback();
      }
    },
    [callback, code]
  );

  useEffect(() => {
    window.addEventListener("keydown", handleKeyPress);
    return () => {
      window.removeEventListener("keydown", handleKeyPress);
    };
  }, [handleKeyPress]);
}
