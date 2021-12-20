import { faUser } from "@fortawesome/free-solid-svg-icons";
import { useState } from "react";
import Button from "../button/Button";
import Modal from "./Modal";
import ModalActions from "./ModalActions";
import ModalBody from "./ModalBody";
import ModalContainer from "./ModalContainer";
import ModalContent from "./ModalContent";
import ModalIcon from "./ModalIcon";

export default {
  Open: () => {
    const [open, setOpen] = useState(true);

    return (
      <>
        <button className="border" onClick={() => setOpen(true)}>
          Open modal
        </button>
        <Modal open={open} close={() => setOpen(false)}>
          <div className="bg-red-300">
            <div className="w-full">asda</div>
          </div>
        </Modal>
      </>
    );
  },
  EmptyContainer: () => {
    const [open, setOpen] = useState(true);

    return (
      <>
        <Button onClick={() => setOpen(true)}>Open modal</Button>
        <Modal open={open} close={() => setOpen(false)}>
          <ModalContainer>no content</ModalContainer>
        </Modal>
      </>
    );
  },
  WithActions: () => {
    const [open, setOpen] = useState(true);

    return (
      <>
        <Button onClick={() => setOpen(true)}>Open modal</Button>
        <Modal open={open} close={() => setOpen(false)}>
          <ModalContainer>
            <ModalActions close={() => setOpen(false)}>
              <Button onClick={() => console.log("clicked")}>Save</Button>
            </ModalActions>
          </ModalContainer>
        </Modal>
      </>
    );
  },
  WithIcon: () => {
    const [open, setOpen] = useState(true);

    return (
      <>
        <Button onClick={() => setOpen(true)}>Open modal</Button>
        <Modal open={open} close={() => setOpen(false)}>
          <ModalContainer>
            <ModalIcon icon={faUser} />
            <ModalActions close={() => setOpen(false)}>
              <Button onClick={() => console.log("clicked")}>Save</Button>
            </ModalActions>
          </ModalContainer>
        </Modal>
      </>
    );
  },
  WithIconAndBody: () => {
    const [open, setOpen] = useState(true);

    return (
      <>
        <Button onClick={() => setOpen(true)}>Open modal</Button>
        <Modal open={open} close={() => setOpen(false)}>
          <ModalContainer>
            <ModalContent>
              <ModalIcon icon={faUser} />
              <ModalBody>this is some content over here</ModalBody>
            </ModalContent>
            <ModalActions close={() => setOpen(false)}>
              <Button onClick={() => console.log("clicked")}>Save</Button>
            </ModalActions>
          </ModalContainer>
        </Modal>
      </>
    );
  }
};
