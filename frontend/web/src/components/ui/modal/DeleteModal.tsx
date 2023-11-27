import { Modal, ModalProps } from "./Modal";
import { DeleteModalContainer } from "./DeleteModalContainer";

interface IDeleteModal extends ModalProps {
  onConfirm?: () => void;
}

export function DeleteModal(props: IDeleteModal) {
  return (
    <Modal open={props.open} close={props.close} className="max-w-md">
      <DeleteModalContainer close={props.close} onConfirm={props.onConfirm}>
        {props.children}
      </DeleteModalContainer>
    </Modal>
  );
}
