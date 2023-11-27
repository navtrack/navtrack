import { useFormikContext } from "formik";
import { useIntl } from "react-intl";
import { TextInput, TextInputProps } from "./TextInput";

export type FormikTextInputProps = TextInputProps & {
  name: string;
};

export function FormikTextInput(props: FormikTextInputProps) {
  const formikContext = useFormikContext();
  const fieldMeta = formikContext.getFieldMeta<string>(props.name);
  const intl = useIntl();

  return (
    <TextInput
      {...props}
      value={fieldMeta.value}
      onChange={props.onChange ?? formikContext.handleChange}
      onBlur={props.onBlur ?? formikContext.handleBlur}
      label={props.label ? intl.formatMessage({ id: props.label }) : undefined}
      placeholder={
        props.placeholder
          ? intl.formatMessage({ id: props.placeholder })
          : undefined
      }
      error={
        fieldMeta && fieldMeta.error && fieldMeta.touched
          ? fieldMeta.error
          : undefined
      }
    />
  );
}
