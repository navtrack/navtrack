import { ReactNode } from "react";
import { RecoilRoot } from "recoil";
import "./index.css";
import "leaflet/dist/leaflet.css";
import "@geoman-io/leaflet-geoman-free";
import "@geoman-io/leaflet-geoman-free/dist/leaflet-geoman.css";
import { IntlProvider } from "react-intl";
import { translations } from "@navtrack/shared/translations";

export default function Decorator({ children }: { children: ReactNode }) {
  return (
    <IntlProvider locale="en" messages={translations["en"]}>
      <RecoilRoot>
        <div className="flex min-h-screen flex-col p-10">
          <div>{children}</div>
        </div>
      </RecoilRoot>
    </IntlProvider>
  );
}
