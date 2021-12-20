import moment from "moment";

export const showDate = (date?: string): string => {
  if (date) {
    return moment(date).format("YYYY-MM-DD");
  }

  return "N/A";
};

export const showTime = (date?: string): string => {
  if (date) {
    return moment(date).format("HH:mm:ss");
  }

  return "N/A";
};

export const showDateTime = (date?: string): string => {
  if (date) {
    return `${showDate(date)} ${showTime(date)}`;
  }

  return "N/A";
};
