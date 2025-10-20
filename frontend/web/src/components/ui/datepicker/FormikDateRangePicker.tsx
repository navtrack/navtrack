import { useFormikContext } from "formik";
import { useIntl } from "react-intl";
import { DateRangePicker, DateRangePickerProps } from "./DateRangePicker";

export type FormikDateRangePickerProps = DateRangePickerProps & {
  startDateName: string;
  endDateName: string;
};

export function FormikDateRangePicker(props: FormikDateRangePickerProps) {
  const formikContext = useFormikContext();
  const startDateFieldMeta = formikContext.getFieldMeta<Date>(
    props.startDateName
  );
  const endDateFieldMeta = formikContext.getFieldMeta<Date>(props.endDateName);
  const intl = useIntl();

  return (
    <DateRangePicker
      {...props}
      value={[startDateFieldMeta.value, endDateFieldMeta.value]}
      onChange={props.onChange}
      label={props.label ? intl.formatMessage({ id: props.label }) : undefined}
    />
  );
}
