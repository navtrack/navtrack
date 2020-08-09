import React from "react";
import classNames from "classnames";

type Props = {
  className: string;
};

export default function AdminFooter(props: Props) {
  const year = new Date().getFullYear();

  return (
    <footer className={classNames("text-xs mt-2 text-right", props.className)}>
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
