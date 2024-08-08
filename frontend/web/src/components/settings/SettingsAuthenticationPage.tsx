import { useContext } from "react";
import { SlotContext } from "../../app/SlotContext";
import { ChangePasswordCard } from "./ChangePasswordCard";

export function SettingsAuthenticationPage() {
  const slots = useContext(SlotContext);

  return (
    <>
      {slots?.settingsPasswordAuthenticationBlock}
      <ChangePasswordCard />
    </>
  );
}
