import { FormattedMessage } from "react-intl";
import Button from "../button/Button";
import { faTrash } from "@fortawesome/free-solid-svg-icons";
import { ReactNode } from "react";
import ModalContainer from "./ModalContainer";
import ModalContent from "./ModalContent";
import ModalIcon from "./ModalIcon";
import ModalBody from "./ModalBody";
import ModalActions from "./ModalActions";

interface IDeleteModalContainer {
  close: () => void;
  onConfirm?: () => void;
  children?: ReactNode;
  loading?: boolean;
}

export default function DeleteModalContainer(props: IDeleteModalContainer) {
  return (
    <ModalContainer>
      <ModalContent>
        <ModalIcon icon={faTrash} />
        <ModalBody> {props.children}</ModalBody>
      </ModalContent>
      <ModalActions close={props.close}>
        <Button
          color="warn"
          type="submit"
          onClick={props.onConfirm}
          loading={props.loading}>
          <FormattedMessage id="generic.delete" />
        </Button>
      </ModalActions>
    </ModalContainer>
  );
}
