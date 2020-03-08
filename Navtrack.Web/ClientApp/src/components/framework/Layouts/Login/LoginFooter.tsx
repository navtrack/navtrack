import React from "react";

export default function LoginFooter() {
  const date = new Date();
  const year = date.getFullYear();

  return (
    <div className="text-muted p-3">
      © {year}{" "}
      <a href="https://www.navtrack.io" target="_blank" rel="noopener noreferrer">
        Navtrack
      </a>
    </div>
  );
}
