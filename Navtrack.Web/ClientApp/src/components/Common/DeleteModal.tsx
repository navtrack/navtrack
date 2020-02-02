import React from "react";

type Props = {
  show: boolean
  setShow: (show: boolean) => void,
  deleteHandler: () => void,
  description?: string
}

export default function DeleteModal(props: Props) {

  // const handleDelete = () => {
  //   props.setShow(false);
  //   props.deleteHandler();
  // }

  return (
    <></>
    // <Modal show={props.show} onHide={() => props.setShow(false)}>
    //     <Modal.Header></Modal.Header>
    //     <div className="text-center">Are you sure you want to delete this item?</div>
    //     {props.description && <div className="text-center">{props.description}</div>}
    //     <Modal.Footer>
    //         <button color="secondary" onClick={() => props.setShow(false)}>Cancel</button>
    //         <button color="primary" onClick={handleDelete}>Delete</button>
    //     </Modal.Footer>
    // </Modal>
  );
}