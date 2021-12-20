import { ReactNode } from "react";

export interface IBadge {
  children: ReactNode;
  onClick?: () => void;
  onCloseClick?: () => void;
  order?: number;
}

export default function Badge(props: IBadge) {
  return (
    <div
      className="cursor-pointer inline-flex items-center py-1 pl-2 pr-1 rounded-full text-xs font-medium bg-gray-200 text-gray-700 hover:bg-gray-300"
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
          className="flex-shrink-0 ml-0.5 h-4 w-4 rounded-full inline-flex items-center justify-center text-gray-600 hover:bg-gray-200 hover:text-gray-500 focus:outline-none focus:bg-gray-500 focus:text-white">
          <svg className="h-2 w-2" stroke="currentColor" fill="none" viewBox="0 0 8 8">
            <path strokeLinecap="round" strokeWidth="1.5" d="M1 1l6 6m0-6L1 7" />
          </svg>
        </button>
      )}
    </div>
  );
}
