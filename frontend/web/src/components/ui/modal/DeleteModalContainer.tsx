import { FormattedMessage } from "react-intl";
import { faTrash } from "@fortawesome/free-solid-svg-icons";
import { ReactNode } from "react";
import { ModalContainer } from "./ModalContainer";
import { ModalContent } from "./ModalContent";
import { ModalIcon } from "./ModalIcon";
import { ModalBody } from "./ModalBody";
import { ModalActions } from "./ModalActions";
import { Button } from "../button/Button";

type DeleteModalContainerProps = {
  close: () => void;
  onConfirm?: () => void;
  children?: ReactNode;
  loading?: boolean;
};

export function DeleteModalContainer(props: DeleteModalContainerProps) {
  return (
    <ModalContainer>
      <ModalContent>
        <ModalIcon icon={faTrash} />
        <ModalBody> {props.children}</ModalBody>
      </ModalContent>
      <ModalActions close={props.close}>
        <Button
          color="error"
          type="submit"
          onClick={props.onConfirm}
          loading={props.loading}>
          <FormattedMessage id="generic.delete" />
        </Button>
      </ModalActions>
    </ModalContainer>
  );
}
