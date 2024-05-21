import { Modal } from "./Modal";
import { useCallback, useState } from "react";
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
  onConfirm?: () => Promise<void> | undefined;
  renderButton?: (open: () => void) => JSX.Element;
  children?: React.ReactNode;
  onClose?: () => void;
  maxWidth?: "lg";
  disabled?: boolean;
  deleteButtonTitle?: string;
};

export function DeleteModal(props: DeleteModalProps) {
  const [open, setOpen] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const close = useCallback(() => {
    setOpen(false);
    props.onClose?.();
  }, [props]);

  const handleConfirm = useCallback(async () => {
    setIsLoading(true);

    if (props.onConfirm !== undefined) {
      await props.onConfirm();
    }

    setIsLoading(false);
    close();
  }, [close, props]);

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
        />
      )}
      {!props.disabled && (
        <Modal
          open={open}
          close={() => {
            if (!isLoading) {
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
            <ModalActions cancel={close} isLoading={isLoading}>
              <Button
                color="error"
                type="submit"
                isLoading={isLoading}
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
