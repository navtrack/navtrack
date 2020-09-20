import moment from "moment";

export const showDate = (date?: Date): string => {
  if (date) {
    return moment(date).format("YYYY-MM-DD HH:mm:ss");
  }

  return "N/A";
};
