import React from "react";
import Modal from "components/framework/Elements/Modal";
import Icon from "components/framework/Util/Icon";
import Button from "components/framework/Elements/Button";

type Props = {
  deleteHandler: () => void;
  description?: string;
  closeModal: () => void;
};

export default function DeleteModal(props: Props) {
  const deleteClickHandler = () => {
    props.closeModal();
    props.deleteHandler();
  };

  return (
    <Modal closeModal={props.closeModal}>
      <div className="font-medium text-lg mb-3">
        <Icon className="fa-trash mr-1" />
        Delete confirmation
      </div>
      <div className="flex flex-col cursor-default text-sm p-3 justify-center">
        <span className="text-center">Are you sure you want to delete this item?</span>
        {props.description && <span className="text-center mt-2">{props.description}</span>}
      </div>
      <div className="flex justify-end mt-3">
        <Button color="secondary" onClick={props.closeModal} className="mr-3">
          Cancel
        </Button>
        <Button color="primary" onClick={deleteClickHandler}>
          Delete
        </Button>
      </div>
    </Modal>
  );
}
