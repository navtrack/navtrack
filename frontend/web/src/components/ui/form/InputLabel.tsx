type InputLabelProps = {
  label?: string;
  name?: string;
};

export function InputLabel(props: InputLabelProps) {
  return (
    <label
      htmlFor={props.name}
      className="block text-sm font-medium leading-6 text-gray-900">
      {props.label}
    </label>
  );
}
