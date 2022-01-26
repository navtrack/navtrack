import { useFormikContext } from "formik";
import { useIntl } from "react-intl";
import { IFormikInput } from "../formik/IFormikInput";
import SelectInput, { ISelectInput } from "./SelectInput";

export default function FormikSelectInput(props: ISelectInput & IFormikInput) {
  const formikContext = useFormikContext();
  const fieldMeta = formikContext.getFieldMeta<string>(`${props.name}`);
  const intl = useIntl();

  return (
    <SelectInput
      items={props.items}
      onChange={(value) => {
        props.onChange?.(value);
        formikContext.setFieldValue(props.name, value);
      }}
      placeholder={props.placeholder}
      value={fieldMeta.value}
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
