import { faEdit } from "@fortawesome/free-solid-svg-icons";
import { useState, useCallback, ReactNode } from "react";
import { FormattedMessage } from "react-intl";
import { ModalActions } from "./ModalActions";
import { ModalContainer } from "./ModalContainer";
import { ModalContent } from "./ModalContent";
import { ModalIcon } from "./ModalIcon";
import { Button } from "../button/Button";
import { Modal } from "./Modal";
import { ModalBody } from "./ModalBody";
import { Form, Formik, FormikHelpers, FormikValues } from "formik";
import { IconProp } from "@fortawesome/fontawesome-svg-core";

type NewEditModalProps<T extends FormikValues> = {
  onConfirm?: () => Promise<void> | undefined;
  renderButton?: (open: () => void) => Element;
  children?: ReactNode;
  maxWidth?: "lg";
  disabled?: boolean;
  deleteButtonTitle?: string;
  buttonLabel?: string;
  initialValues: T;
  validationSchema?: any;
  onSubmit: (
    values: T,
    formikHelpers: FormikHelpers<T>,
    close: () => void
  ) => Promise<void>;
  icon: IconProp;
  title: string;
  isLoading?: boolean;
};

export function NewEditModal<T extends FormikValues>(
  props: NewEditModalProps<T>
) {
  const [open, setOpen] = useState(false);

  const onSubmit = useCallback(
    async (values: T, formikHelpers: FormikHelpers<T>) => {
      if (props.onSubmit !== undefined) {
        await props.onSubmit(values, formikHelpers, () => setOpen(false));
      }
    },
    [props]
  );

  return (
    <>
      <Button
        icon={faEdit}
        color="white"
        onClick={() => setOpen(true)}
        disabled={props.disabled}
        title={props.deleteButtonTitle}
        size="xs">
        {props.buttonLabel && <FormattedMessage id={props.buttonLabel} />}
      </Button>
      {!props.disabled && (
        <Modal
          open={open}
          close={() => setOpen(false)}
          className="w-full max-w-md">
          <Formik<T>
            initialValues={props.initialValues}
            validationSchema={props.validationSchema}
            onSubmit={onSubmit}>
            {() => (
              <Form>
                <ModalContainer>
                  <ModalContent>
                    <ModalIcon icon={props.icon} />
                    <ModalBody>
                      <div className="text-md font-medium">
                        <FormattedMessage id={props.title} />
                      </div>
                      <div className="mt-4 space-y-4">{props.children}</div>
                    </ModalBody>
                  </ModalContent>
                  <ModalActions
                    cancel={() => setOpen(false)}
                    isLoading={props.isLoading}>
                    <Button
                      color="primary"
                      type="submit"
                      isLoading={props.isLoading}>
                      <FormattedMessage id="generic.save" />
                    </Button>
                  </ModalActions>
                </ModalContainer>
              </Form>
            )}
          </Formik>
        </Modal>
      )}
    </>
  );
}
