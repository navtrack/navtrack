import { useFormikContext } from "formik";
import { useIntl } from "react-intl";
import { Autocomplete, AutocompleteProps } from "./Autocomplete";

export type FormikCustomSelectProps = AutocompleteProps & {
  name: string;
};

export function FormikAutocomplete(props: FormikCustomSelectProps) {
  const formikContext = useFormikContext();
  const fieldMeta = formikContext.getFieldMeta<string>(props.name);
  const intl = useIntl();

  return (
    <Autocomplete
      {...props}
      onChange={(value) => {
        props.onChange?.(value);
        formikContext.setFieldValue(props.name, value);
      }}
      value={fieldMeta.value}
      placeholder={
        props.placeholder
          ? intl.formatMessage({ id: props.placeholder })
          : undefined
      }
      label={props.label}
      error={
        fieldMeta && fieldMeta.error && fieldMeta.touched
          ? fieldMeta.error
          : undefined
      }
    />
  );
}
