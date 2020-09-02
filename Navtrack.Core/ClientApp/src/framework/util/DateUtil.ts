export const showDate = (date?: Date): string => {
  if (date) {
    return date.toString();
  }

  return "N/A";
};