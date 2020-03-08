import React from "react";
import classNames from "classnames";

type Props = {
  className: string;
};

export default function AdminFooter(props: Props) {
  const date = new Date();
  const year = date.getFullYear();

  return (
    <footer className={classNames("text-sm mt-5", props.className)}>
      &copy; {year}
      <a
        href="https://navtrack.io"
        className="font-weight-bold ml-1"
        target="_blank"
        rel="noopener noreferrer">
        Navtrack
      </a>
    </footer>
  );
}
