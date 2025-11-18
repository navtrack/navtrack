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
      { label: "generic.distance", value: "distance" },
      { label: "generic.duration", value: "duration" },
      { label: "generic.average-speed", value: "average-speed" },
      { label: "generic.max-speed", value: "max-speed" }
    ];

    return <ButtonGroup options={options} />;
  }
};
