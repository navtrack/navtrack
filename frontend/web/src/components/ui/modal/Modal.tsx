import {
  Transition,
  Dialog,
  TransitionChild,
  DialogBackdrop
} from "@headlessui/react";
import { classNames } from "@navtrack/shared/utils/tailwind";
import { Fragment, ReactNode } from "react";
import { ZINDEX_MODAL } from "../../../constants";

type ModalProps = {
  open: boolean;
  close: () => void;
  children?: ReactNode;
  className?: string;
};

export function Modal(props: ModalProps) {
  return (
    <Transition show={props.open} as={Fragment}>
      <Dialog
        className="relative"
        style={{ zIndex: ZINDEX_MODAL }}
        onClose={() => props.close()}>
        <DialogBackdrop className="fixed inset-0 bg-black/30" />
        <div className="fixed inset-0 flex w-screen items-center justify-center p-4">
          <TransitionChild
            as={Fragment}
            enter="ease-out duration-200"
            enterFrom="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
            enterTo="opacity-100 translate-y-0 sm:scale-100"
            leave="ease-in duration-100"
            leaveFrom="opacity-100 translate-y-0 sm:scale-100"
            leaveTo="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95">
            <div
              className={classNames(
                "my-8 inline-block transform rounded-lg bg-white text-left align-middle shadow-xl transition-all",
                props.className
              )}>
              {props.children}
            </div>
          </TransitionChild>
        </div>
      </Dialog>
    </Transition>
  );
}
