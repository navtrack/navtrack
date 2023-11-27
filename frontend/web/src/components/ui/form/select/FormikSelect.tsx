import { useFormikContext } from "formik";
import { useIntl } from "react-intl";
import { Select, SelectProps } from "./Select";

export type FormikSelectProps = SelectProps & {
  name: string;
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
