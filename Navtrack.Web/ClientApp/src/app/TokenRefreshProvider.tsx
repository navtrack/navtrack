import { useTokenRefresh } from "../hooks/authentication/useTokenRefresh";

const TokenRefreshProvider: React.FC = (props) => {
  useTokenRefresh();

  return <>{props.children}</>;
};

export default TokenRefreshProvider;
