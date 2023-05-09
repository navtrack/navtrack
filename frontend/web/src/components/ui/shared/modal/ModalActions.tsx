import { ReactNode } from "react";
import { FormattedMessage } from "react-intl";
import { Button } from "../button/Button";

interface IModalActions {
  children?: ReactNode;
  close?: () => void;
}

export function ModalActions(props: IModalActions) {
  return (
    <div className="flex justify-end space-x-3 bg-gray-50 px-4 py-3">
      {props.close !== undefined && (
        <Button onClick={props.close} color="white">
          <FormattedMessage id="generic.cancel" />
        </Button>
      )}
      {props.children}
    </div>
  );
}
