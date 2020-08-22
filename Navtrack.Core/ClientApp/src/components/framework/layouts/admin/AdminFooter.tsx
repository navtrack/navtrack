import React from "react";

export default function AdminFooter() {
  const year = new Date().getFullYear();

  return (
    <footer className="text-xs font-medium text-right mx-5 my-2">
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
