import colors from "tailwindcss/colors";

export function c(
  check: boolean | string | undefined,
  trueValue: string,
  falseValue: string = ""
) {
  return !!check ? trueValue : falseValue ?? "";
}

type ClassName = string | undefined;

export function classNames(...classNames: ClassName[]) {
  return classNames.join(" ");
}

export const TailwindColors = colors;
