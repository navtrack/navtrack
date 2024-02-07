import { useFormikContext } from "formik";
import { DatePicker, DatePickerProps } from "./DatePicker";
import { useIntl } from "react-intl";

export type FormikDatePickerProps = DatePickerProps & {
  name: string;
};

export function FormikDatePicker(props: FormikDatePickerProps) {
  const formikContext = useFormikContext();
  const fieldMeta = formikContext.getFieldMeta<Date>(props.name);
  const intl = useIntl();

  return (
    <DatePicker
      {...props}
      value={fieldMeta.value}
      onChange={props.onChange ?? formikContext.handleChange}
      label={props.label ? intl.formatMessage({ id: props.label }) : undefined}
    />
  );
}
