import { SlotContext } from "./SlotContext";

type SlotProviderProps = {
  children?: React.ReactNode;
};

export function SlotProvider(props: SlotProviderProps) {
  return (
    <SlotContext.Provider value={{}}>{props.children}</SlotContext.Provider>
  );
}
