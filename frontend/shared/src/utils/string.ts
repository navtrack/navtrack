export function capitalizeFirstLetter(value?: string) {
  return value ? value.charAt(0).toUpperCase() + value.slice(1) : "";
}
