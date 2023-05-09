import { ReactNode } from "react";

export interface IBadge {
  children: ReactNode;
  onClick?: () => void;
  onCloseClick?: () => void;
  order?: number;
}

export function Badge(props: IBadge) {
  return (
    <div
      className="inline-flex cursor-pointer items-center rounded-full bg-gray-200 py-1 pl-2 pr-1 text-xs font-medium text-gray-700 hover:bg-gray-300"
      onClick={props.onClick}
      style={{ order: props.order }}>
      <div className="mr-1">{props.children}</div>
      {props.onCloseClick !== undefined && (
        <button
          onClick={(e) => {
            e.stopPropagation();
            props.onCloseClick?.();
          }}
          type="button"
          className="ml-0.5 inline-flex h-4 w-4 flex-shrink-0 items-center justify-center rounded-full text-gray-600 hover:bg-gray-200 hover:text-gray-500 focus:bg-gray-500 focus:text-white focus:outline-none">
          <svg
            className="h-2 w-2"
            stroke="currentColor"
            fill="none"
            viewBox="0 0 8 8">
            <path
              strokeLinecap="round"
              strokeWidth="1.5"
              d="M1 1l6 6m0-6L1 7"
            />
          </svg>
        </button>
      )}
    </div>
  );
}
