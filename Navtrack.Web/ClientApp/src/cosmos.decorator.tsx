import { ReactNode } from "react";
import { RecoilRoot } from "recoil";
import "./index.css";
import "leaflet/dist/leaflet.css";
import "@geoman-io/leaflet-geoman-free";
import "@geoman-io/leaflet-geoman-free/dist/leaflet-geoman.css";
import { IntlProvider } from "react-intl";
import translations from "./translations";

export default function Decorator({ children }: { children: ReactNode }) {
  return (
    <IntlProvider locale="en" messages={translations["en"]}>
      <RecoilRoot>
        <div className="p-10 min-h-screen flex flex-col">
          <div>{children}</div>
        </div>
      </RecoilRoot>
    </IntlProvider>
  );
}
