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

type DeleteModalProps = {
  onConfirm?: () => void;
  isLoading?: boolean;
  renderButton?: (open: () => void) => JSX.Element;
  children?: React.ReactNode;
  onClose?: () => void;
};

export function DeleteModal(props: DeleteModalProps) {
  const [open, setOpen] = useState(false);

  const close = useCallback(() => {
    setOpen(false);
    props.onClose?.();
  }, [props]);

  return (
    <>
      {props.renderButton ? (
        props.renderButton(() => setOpen(true))
      ) : (
        <Button icon={faTrashAlt} color="error" onClick={() => setOpen(true)} />
      )}
      <Modal open={open} close={close} className="max-w-md">
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
          <ModalActions cancel={close}>
            <Button
              color="error"
              type="submit"
              isLoading={props.isLoading}
              onClick={() => props.onConfirm?.()}>
              <FormattedMessage id="generic.delete" />
            </Button>
          </ModalActions>
        </ModalContainer>
      </Modal>
    </>
  );
}
