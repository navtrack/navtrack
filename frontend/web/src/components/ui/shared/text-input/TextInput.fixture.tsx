import { TextInput } from "./TextInput";
import { TextInputLeftAddon } from "./TextInputLeftAddon";
import { TextInputRightAddon } from "./TextInputRightAddon";

export default {
  Basic: <TextInput name="basic" />,
  BasicDisabled: <TextInput name="basic" disabled value="1234" />,
  WithLabel: <TextInput name="basic" label="Email" />,
  WithPlaceholder: <TextInput name="basic" placeholder="placeholder" />,
  WithLeftAddon: (
    <TextInput
      name="basic"
      placeholder="placeholder"
      className="pl-12"
      leftAddon={<TextInputLeftAddon>km/h</TextInputLeftAddon>}
    />
  ),
  WithRightAddon: (
    <TextInput
      placeholder="placeholder"
      className="pr-12"
      rightAddon={<TextInputRightAddon>km/h</TextInputRightAddon>}
    />
  )
};
