export function formatErrorMessage(message?: string | null) {
  return message ? `errors.${message}` : "errors";
}
