import { useFixtureInput } from "react-cosmos/client";
import { ButtonGroup, ButtonGroupOption } from "./ButtonGroup";

type ChartType = "distance" | "duration" | "average-speed" | "max-speed";

export default {
  Default: () => {
    const [selected, setSelected] = useFixtureInput<ChartType>(
      "selected",
      "distance"
    );

    const options: ButtonGroupOption[] = [
      { label: "distance", value: "distance" },
      { label: "duration", value: "duration" },
      { label: "average-speed", value: "average-speed" },
      { label: "max-speed", value: "max-speed" }
    ];

    return <ButtonGroup options={options} />;
  }
};
