import { useLayoutEffect, useState } from "react";

type Size = {
  width: number;
  height: number;
};
export function useElementSize(ref?: React.RefObject<HTMLElement | null>) {
  const [size, setSize] = useState<Size>({
    width: 0,
    height: 0
  });

  useLayoutEffect(() => {
    if (!ref) return;

    const updateSize = () => {
      if (ref?.current) {
        setSize({
          width: ref.current.offsetWidth,
          height: ref.current.offsetHeight
        });
      }
    };

    updateSize(); // Set initial size

    const observer = new ResizeObserver(() => updateSize());
    if (ref?.current) observer.observe(ref?.current);

    return () => {
      observer.disconnect();
    };
  }, [ref]);

  return size;
}
