import { useFormikContext } from "formik";
import { useIntl } from "react-intl";
import TextInput, { ITextInput } from "./TextInput";

export default function FormikTextInput(props: ITextInput) {
  const formikContext = useFormikContext();
  const fieldMeta = formikContext.getFieldMeta(`${props.name}`);
  const intl = useIntl();

  return (
    <TextInput
      {...props}
      value={`${fieldMeta.value}`}
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
