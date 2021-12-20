import { useFormikContext } from "formik";
import { useIntl } from "react-intl";
import { IFormikInput } from "../formik/IFormikInput";
import Select, { ISelect } from "./Select";

export default function FormikSelect(props: ISelect & IFormikInput) {
  const formikContext = useFormikContext();
  const fieldMeta = formikContext.getFieldMeta<string>(`${props.name}`);
  const intl = useIntl();

  return (
    <Select
      {...props}
      onChange={(value) =>
        props.onChange ?? formikContext.setFieldValue(props.name, value)
      }
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
