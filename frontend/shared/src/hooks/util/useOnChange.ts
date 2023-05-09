import { useEffect, useState } from "react";

export function useOnChange<T>(
  value: T,
  callback: (previous: T, current: T) => void
) {
  const [state, setState] = useState(value);

  useEffect(() => {
    if (value !== state) {
      callback(state, value);
      setState(value);
    }
  }, [callback, state, value]);
}
