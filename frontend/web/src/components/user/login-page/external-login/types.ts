export type CustomExternalLoginButtonProps = {
  login: (code: string, grantType: "apple" | "microsoft" | "google") => void;
};
