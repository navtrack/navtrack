import { useEffect } from "react";

export const useOnArrowDown = (listener: () => void): void => {
  useOnKeyDown(listener, 40);
};

export const useOnArrowUp = (listener: () => void): void => {
  useOnKeyDown(listener, 38);
};

const useOnKeyDown = (listener: () => void, keyCode: number) => {
  useEffect(() => {
    const f = (e: KeyboardEvent) => {
      if (e.keyCode === keyCode) {
        e.preventDefault();
        listener();
      }
    };

    window.addEventListener("keydown", f);
    return () => {
      window.removeEventListener("keydown", f);
    };
  }, [listener, keyCode]);
};
