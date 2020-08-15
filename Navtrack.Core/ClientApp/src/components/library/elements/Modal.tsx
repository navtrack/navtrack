import React, { ReactNode } from "react";

type Props = {
  children: ReactNode;
  closeModal: () => void;
  onContentClick?: () => void;
};

export default function Modal(props: Props) {
  return (
    <div
      className="fixed left-0 top-0 w-full h-full z-50 flex flex-col items-center justify-center bg-gray-100 fadeIn animated faster"
      style={{ backgroundColor: "rgba(26, 32, 44, 0.15)" }}
      onClick={props.closeModal}>
      <div
        className="shadow rounded p-4 bg-white"
        onClick={e => {
          e.stopPropagation();
          e.nativeEvent.stopImmediatePropagation();
          if (props.onContentClick) props.onContentClick();
        }}>
        {props.children}
      </div>
    </div>
  );
}
