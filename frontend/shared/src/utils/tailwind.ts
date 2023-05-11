export function c(check: boolean, trueValue: string, falseValue: string = "") {
  return check ? trueValue : falseValue ?? "";
}

export function classNames(...classNames: string[]) {
  return classNames.join(" ");
}
