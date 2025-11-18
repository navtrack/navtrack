import colors from "tailwindcss/colors";

type ClassName = string | undefined;

export function c(
  check: boolean | string | undefined,
  trueValue: ClassName,
  falseValue: ClassName = "",
  bothValue: ClassName = ""
) {
  return classNames(!!check ? trueValue : falseValue, bothValue);
}

export function classNames(...classNames: ClassName[]) {
  return classNames.join(" ");
}

export type TailwindColorType = typeof colors;

export const TailwindColors: TailwindColorType = colors;
