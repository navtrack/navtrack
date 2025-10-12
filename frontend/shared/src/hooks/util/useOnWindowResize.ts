import { useEffect } from "react";

export function useOnWindowResize(handler: () => void) {
  useEffect(() => {
    const handleResize = () => {
      handler();
    };
    handleResize();
    window.addEventListener("resize", handleResize);

    return () => window.removeEventListener("resize", handleResize);
  }, [handler]);
}
