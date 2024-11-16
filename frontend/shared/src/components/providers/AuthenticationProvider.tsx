import { ReactNode, useEffect } from "react";
import { useAuthentication } from "../../hooks/app/authentication/useAuthentication";

type AuthenticationProps = {
  children: ReactNode;
};

export function AuthenticationProvider(props: AuthenticationProps) {
  const authentication = useAuthentication();

  useEffect(() => {
    if (!authentication.state.initialized) {
      authentication.initialize();
    }
  }, [authentication]);

  return <>{authentication.state.initialized && props.children}</>;
}
