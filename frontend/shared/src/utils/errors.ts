export function formatErrorMessage(message?: string | null) {
  return `errors.${message ?? "generic"}`;
}
