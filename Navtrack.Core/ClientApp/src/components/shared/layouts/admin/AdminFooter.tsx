import React from "react";
import { useContext } from "react";
import { AppContext } from "../../../../services/appContext/AppContext";

export default function AdminFooter() {
  const { appContext } = useContext(AppContext);
  const year = new Date().getFullYear();

  return (
    <footer
      className="text-xs text-right px-2 py-1 z-0 shadow-md"
      style={{
        backdropFilter: appContext.mapIsVisible ? "blur(10px)" : "",
        backgroundColor: appContext.mapIsVisible
          ? "rgba(255, 255, 255, 0.5)"
          : "rgba(255, 255, 255, 0.5)"
      }}>
      &copy; {year}
      <a
        href="https://codeagency.com"
        className="font-weight-bold ml-1"
        target="_blank"
        rel="noopener noreferrer">
        CodeAgency
      </a>
    </footer>
  );
}
