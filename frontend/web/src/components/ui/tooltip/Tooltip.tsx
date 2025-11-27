import { useState, useRef, useEffect, ReactNode } from "react";

type TooltipProps = {
  content: ReactNode;
  children?: ReactNode;
  className?: string;
};

export function Tooltip(props: TooltipProps) {
  const [isVisible, setIsVisible] = useState(false);
  const [position, setPosition] = useState<{ top: number; left: number }>({
    top: 0,
    left: 0
  });

  const triggerRef = useRef<HTMLDivElement>(null);
  const tooltipRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (!isVisible || !triggerRef.current || !tooltipRef.current) return;

    const trigger = triggerRef.current.getBoundingClientRect();
    const tooltip = tooltipRef.current.getBoundingClientRect();

    const top = trigger.top + window.scrollY - tooltip.height - 8;
    const left =
      trigger.left + window.scrollX + trigger.width / 2 - tooltip.width / 2;

    setPosition({
      top,
      left: Math.max(8, left)
    });
  }, [isVisible]);

  return (
    <div
      ref={triggerRef}
      className={`relative inline-block ${props.className}`}
      onMouseEnter={() => setIsVisible(true)}
      onMouseLeave={() => setIsVisible(false)}>
      {props.children}
      {isVisible && (
        <div
          ref={tooltipRef}
          className="fixed z-50 px-3 py-2 text-sm font-medium text-white bg-gray-900 rounded-lg shadow-lg pointer-events-none whitespace-nowrap"
          style={{
            top: `${position.top}px`,
            left: `${position.left}px`
          }}>
          {props.content}
          <div
            className="absolute top-full left-1/2 -translate-x-1/2 -mt-1"
            style={{
              borderLeft: "8px solid transparent",
              borderRight: "8px solid transparent",
              borderTop: "8px solid rgb(31 41 55)" // tailwind gray-900
            }}
          />
        </div>
      )}
    </div>
  );
}
