import React, { useState, useEffect } from "react";

const useClickOutside = (): [boolean, 
  (e: React.MouseEvent<Element, MouseEvent>) => void, 
  () => void] => {
  const [isVisible, setIsVisible] = useState(false);

  const show = (e: React.MouseEvent) => {
    e.stopPropagation();
    e.nativeEvent.stopImmediatePropagation();
    setIsVisible(true);
  }
  const hide = () => setIsVisible(false);

  useEffect(() => {
    function handleClickOutside() {
      setIsVisible(false);
    }

    document.addEventListener("click", handleClickOutside);

    return () => {
      document.removeEventListener("click", handleClickOutside);
    }
  });

  return [isVisible, show, hide];
}

export default useClickOutside;