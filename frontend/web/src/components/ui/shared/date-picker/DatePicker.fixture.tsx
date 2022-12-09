import { useState } from "react";
import DatePicker from "./DatePicker";

export default {
  Default: () => {
    const [value, setValue] = useState<Date | null>(new Date());

    return <DatePicker value={value} onChange={setValue} />;
  }
};
