import { useFormikContext } from "formik";
import { useIntl } from "react-intl";
import { IFormikInput } from "../formik/IFormikInput";
import SelectInput, { ISelectInput } from "./SelectInput";

export default function FormikSelectInput<T>(
  props: ISelectInput<T> & IFormikInput
) {
  const formikContext = useFormikContext();
  const fieldMeta = formikContext.getFieldMeta(`${props.name}`);
  const intl = useIntl();

  return (
    <SelectInput
      {...props}
      onChange={(value) => {
        props.onChange?.(value);
        formikContext.setFieldValue(props.name, value);
      }}
      onBlur={props.onBlur ?? formikContext.handleBlur}
      label={props.label ? intl.formatMessage({ id: props.label }) : undefined}
      error={
        fieldMeta && fieldMeta.error && fieldMeta.touched
          ? fieldMeta.error
          : undefined
      }
    />
  );
}
