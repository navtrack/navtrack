import { useRecoilValue } from "recoil";
import { configState } from "./state";

export const useConfig = () => {
  const state = useRecoilValue(configState);

  return state.config;
};
