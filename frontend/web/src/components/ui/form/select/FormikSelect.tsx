import { useFormikContext } from "formik";
import { useIntl } from "react-intl";
import { Select, SelectProps } from "./Select";

export type FormikSelectProps = SelectProps & {
  name: string;
  formatOptionLabel?: boolean;
};

export function FormikSelect(props: FormikSelectProps) {
  const formikContext = useFormikContext();
  const fieldMeta = formikContext.getFieldMeta<string>(props.name);
  const intl = useIntl();

  return (
    <Select
      {...props}
      value={fieldMeta.value}
      onChange={(value) =>
        props.onChange ?? formikContext.setFieldValue(props.name, value)
      }
      options={props.options.map((option) => ({
        label:
          props.formatOptionLabel !== false
            ? intl.formatMessage({ id: option.label })
            : option.label,
        value: option.value
      }))}
      onBlur={props.onBlur ?? formikContext.handleBlur}
      label={props.label}
      error={
        fieldMeta && fieldMeta.error && fieldMeta.touched
          ? fieldMeta.error
          : undefined
      }
    />
  );
}
