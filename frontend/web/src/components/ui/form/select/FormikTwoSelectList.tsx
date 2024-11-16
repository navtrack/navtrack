import { useCallback, useEffect, useMemo, useState } from "react";
import { Select, SelectOption } from "./Select";
import { Button } from "../../button/Button";
import {
  faArrowLeftLong,
  faArrowRightLong
} from "@fortawesome/free-solid-svg-icons";

export type TwoSelectListProps = {
  values?: string[];
  options: SelectOption[];
  onChange?: (value: string[]) => void;
  size?: number;
  leftLabel?: string;
  rightLabel?: string;
  className?: string;
};

function sort(list: SelectOption[]) {
  return list.sort((a, b) => a.label.localeCompare(b.label));
}

export function FormikTwoSelectList(props: TwoSelectListProps) {
  const [selectedLeftOptions, setSelectedLeftOptions] = useState<string[]>([]);
  const [selectedRightOptions, setSelectedRightOptions] = useState<string[]>(
    []
  );

  const rightOptions = useMemo(
    () => sort(props.options.filter((x) => props.values?.includes(x.value))),
    [props.options, props.values]
  );

  const leftOptions = useMemo(
    () =>
      sort(
        props.options.filter(
          (x) => !rightOptions.map((x) => x.value).includes(x.value)
        )
      ),
    [props.options, rightOptions]
  );

  useEffect(() => {
    if (rightOptions.length > 0 && selectedRightOptions.length === 0) {
      setSelectedRightOptions([rightOptions[0].value]);
    }
  }, [rightOptions, selectedRightOptions.length]);

  useEffect(() => {
    if (leftOptions.length > 0 && selectedLeftOptions.length === 0) {
      setSelectedLeftOptions([leftOptions[0].value]);
    }
  }, [leftOptions, selectedLeftOptions.length]);

  const moveRight = useCallback(() => {
    const newRightOptions = [
      ...selectedLeftOptions,
      ...rightOptions.map((x) => x.value)
    ];
    props.onChange?.(newRightOptions);
    setSelectedLeftOptions([]);
    setSelectedRightOptions([]);
  }, [props, rightOptions, selectedLeftOptions]);

  const moveLeft = useCallback(() => {
    const newRightOptions = rightOptions.filter(
      (x) => !selectedRightOptions.includes(x.value)
    );
    props.onChange?.(newRightOptions.map((x) => x.value));
    setSelectedLeftOptions([]);
    setSelectedRightOptions([]);
  }, [props, rightOptions, selectedRightOptions]);

  return (
    <div className="grid grid-cols-11 gap-4">
      <div className="col-span-5">
        <Select
          label={props.leftLabel}
          options={leftOptions}
          size={props.size}
          multiple
          values={selectedLeftOptions}
          onChangeMultiple={(value) => setSelectedLeftOptions(value)}
          className={props.className}
        />
      </div>
      <div className="col-span-1 flex flex-col justify-center space-y-2">
        <Button color="white" onClick={moveRight} icon={faArrowRightLong} />
        <Button color="white" onClick={moveLeft} icon={faArrowLeftLong} />
      </div>
      <div className="col-span-5">
        <Select
          label={props.rightLabel}
          options={rightOptions}
          size={props.size}
          multiple
          values={selectedRightOptions}
          onChangeMultiple={(value) => setSelectedRightOptions(value)}
          className={props.className}
        />
      </div>
    </div>
  );
}
