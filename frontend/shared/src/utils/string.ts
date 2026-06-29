export function capitalizeFirstLetter(value?: string) {
  return value ? value.charAt(0).toUpperCase() + value.slice(1) : "";
}

export function toCamelCase(value: string) {
  return value.charAt(0).toLowerCase() + value.slice(1);
}
