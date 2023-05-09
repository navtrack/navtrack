import { RefObject, useState, useCallback, useEffect } from "react";

export const useHTMLElementSize = (elementRef: RefObject<HTMLElement>) => {
  const [width, setWidth] = useState(0);
  const [height, setHeight] = useState(0);
  const [init, setInit] = useState(false);

  const handleResize = useCallback(() => {
    if (elementRef.current !== null) {
      setWidth(elementRef.current.offsetWidth);
      setHeight(elementRef.current.offsetHeight);
    }
  }, [elementRef]);

  useEffect(() => {
    if (!init) {
      setInit(true);
      handleResize();
    }

    window.addEventListener("resize", handleResize);

    return () => {
      window.removeEventListener("resize", handleResize);
    };
  }, [handleResize, init, elementRef]);

  return { width, height };
};
