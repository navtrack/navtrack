export interface ICustomExternalLoginButton {
  login: (code: string, grantType: "apple" | "microsoft" | "google") => void;
}
