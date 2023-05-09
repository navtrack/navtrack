import { ReactNode, useEffect } from "react";
import { useGetEnvironmentQuery } from "../hooks/queries/useGetEnvironmentQuery";
import { useRecoilState } from "recoil";
import { environmentAtom } from "../state/environment";

type EnvironmentFetcherProps = {
  children: ReactNode;
};

export function EnvironmentFetcher(props: EnvironmentFetcherProps) {
  const environment = useGetEnvironmentQuery();
  const [state, setState] = useRecoilState(environmentAtom);

  useEffect(() => {
    if (!state.initialized && environment.data) {
      setState({ settings: environment.data, initialized: true });
    }
  }, [setState, environment.data, state.initialized]);

  return <>{props.children}</>;
}
