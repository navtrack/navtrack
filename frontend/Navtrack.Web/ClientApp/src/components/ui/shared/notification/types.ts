import { SnackbarType } from "../snackbar/Snackbar";

export type Notification = {
  type: SnackbarType;
  title?: string;
  description: string;
};
