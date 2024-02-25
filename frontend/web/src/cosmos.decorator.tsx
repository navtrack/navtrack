import "@geoman-io/leaflet-geoman-free";
import "@geoman-io/leaflet-geoman-free/dist/leaflet-geoman.css";
import { translations } from "@navtrack/shared/translations";
import "leaflet/dist/leaflet.css";
import { ReactNode } from "react";
import { IntlProvider } from "react-intl";
import { RecoilRoot } from "recoil";
import "./index.css";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

const queryClient = new QueryClient();

export default function Decorator({ children }: { children: ReactNode }) {
  return (
    <IntlProvider locale="en" messages={translations["en"]}>
      <QueryClientProvider client={queryClient}>
        <RecoilRoot>
          <div className="flex min-h-screen flex-col p-10">{children}</div>
        </RecoilRoot>
      </QueryClientProvider>
    </IntlProvider>
  );
}
