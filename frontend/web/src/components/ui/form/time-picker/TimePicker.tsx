import { TextInput, TextInputProps } from "../text-input/TextInput";

export function TimePicker(props: TextInputProps) {
  return <TextInput {...props} type="time" />;
}
