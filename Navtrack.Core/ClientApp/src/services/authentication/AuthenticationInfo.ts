export type AuthenticationInfo = {
  authenticated: boolean;
  email: string; // TODO: remove
  access_token: string;
  refresh_token: string;
  expiry_date: string;
  session_expired: boolean; // TODO: remove
};

export const InitialAuthenticationInfo: AuthenticationInfo = {
  authenticated: false,
  email: "",
  access_token: "",
  refresh_token: "",
  expiry_date: "",
  session_expired: false
};
