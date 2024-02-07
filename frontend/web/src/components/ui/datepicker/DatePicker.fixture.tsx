import { useState } from "react";
import { DatePicker } from "./DatePicker";

export default {
  Default: () => {
    const [value, setValue] = useState(new Date());

    return <DatePicker value={value} onChange={setValue} />;
  }
};
