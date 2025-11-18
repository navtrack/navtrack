import { useRef, useState, useMemo, useEffect } from "react";

export function useOnScreen() {
  const ref = useRef<HTMLDivElement>(null);
  const [isVisible, setIntersecting] = useState(false);

  const observer = useMemo(
    () =>
      new IntersectionObserver(([entry]) =>
        setIntersecting(entry.isIntersecting)
      ),
    []
  );

  useEffect(() => {
    if (ref.current) observer.observe(ref.current);
    return () => observer.disconnect();
  }, [observer, ref]);

  return { isVisible, ref };
}
