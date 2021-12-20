import Snackbar from "./Snackbar";

export default {
  Error: (
    <Snackbar
      show
      type="error"
      title="Error"
      description="There was an error while adding your asset, please try again."
    />
  ),
  Info: <Snackbar show type="info" title="Information" description="I just wanted to say hello!" />,
  Success: (
    <Snackbar
      show
      type="success"
      title="Asset added"
      description="Your asset was added successfully."
    />
  ),
  NoTitle: <Snackbar show type="success" description="Asset added successfully!" />,
  NoTitleLongerText: (
    <Snackbar
      show
      type="success"
      description="Asset added successfully! You can now start tracking your asset."
    />
  )
};
