import { useFormikContext } from "formik";
import { useIntl } from "react-intl";
import { CustomSelect, CustomSelectProps } from "./CustomSelect";

export type FormikCustomSelectProps = CustomSelectProps & {
  name: string;
};

export function FormikCustomSelect(props: FormikCustomSelectProps) {
  const formikContext = useFormikContext();
  const fieldMeta = formikContext.getFieldMeta<string>(props.name);
  const intl = useIntl();

  return (
    <CustomSelect
      options={props.options}
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
