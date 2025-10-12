import { Modal } from "./Modal";
import { ReactNode, useCallback, useState } from "react";
import { faTrash, faTrashAlt } from "@fortawesome/free-solid-svg-icons";
import { ModalActions } from "./ModalActions";
import { ModalContainer } from "./ModalContainer";
import { ModalContent } from "./ModalContent";
import { ModalIcon } from "./ModalIcon";
import { FormattedMessage } from "react-intl";
import { ModalBody } from "./ModalBody";
import { Button } from "../button/Button";
import { c, classNames } from "@navtrack/shared/utils/tailwind";

type DeleteModalProps = {
  onConfirm?: (close: () => void) => Promise<void> | undefined;
  renderButton?: (open: () => void) => ReactNode;
  children?: ReactNode;
  onClose?: () => void;
  maxWidth?: "lg";
  disabled?: boolean;
  deleteButtonTitle?: string;
  buttonLabel?: string;
  isLoading?: boolean;
  autoClose?: boolean;
};

export function DeleteModal(props: DeleteModalProps) {
  const [open, setOpen] = useState(false);

  const close = useCallback(() => {
    setOpen(false);
    props.onClose?.();
  }, [props]);

  const handleConfirm = useCallback(async () => {
    try {
      if (props.onConfirm !== undefined) {
        await props.onConfirm(() => setOpen(false));
      }
    } finally {
      if (props.autoClose === undefined || props.autoClose) {
        setOpen(false);
      }
    }
  }, [props]);

  return (
    <>
      {props.renderButton ? (
        props.renderButton(() => setOpen(true))
      ) : (
        <Button
          icon={faTrashAlt}
          color="error"
          onClick={() => setOpen(true)}
          disabled={props.disabled}
          title={props.deleteButtonTitle}
          size="xs">
          {props.buttonLabel && <FormattedMessage id={props.buttonLabel} />}
        </Button>
      )}
      {!props.disabled && (
        <Modal
          open={open}
          close={() => {
            if (!props.isLoading) {
              close();
            }
          }}
          className={classNames(
            c(props.maxWidth === "lg", "max-w-lg", "max-w-md")
          )}>
          <ModalContainer>
            <ModalContent>
              <ModalIcon icon={faTrash} />
              <ModalBody>
                <div className="mb-4 font-semibold">
                  <FormattedMessage id="shared.delete-modal.title" />
                </div>
                <div className="text-sm">{props.children}</div>
              </ModalBody>
            </ModalContent>
            <ModalActions cancel={close} isLoading={props.isLoading}>
              <Button
                color="error"
                type="submit"
                isLoading={props.isLoading}
                onClick={handleConfirm}>
                <FormattedMessage id="generic.delete" />
              </Button>
            </ModalActions>
          </ModalContainer>
        </Modal>
      )}
    </>
  );
}
