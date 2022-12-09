import Modal, { IModal } from "./Modal";
import DeleteModalContainer from "./DeleteModalContainer";

interface IDeleteModal extends IModal {
  onConfirm?: () => void;
}

export default function DeleteModal(props: IDeleteModal) {
  return (
    <Modal open={props.open} close={props.close} className="max-w-md">
      <DeleteModalContainer close={props.close} onConfirm={props.onConfirm}>
        {props.children}
      </DeleteModalContainer>
    </Modal>
  );
}
