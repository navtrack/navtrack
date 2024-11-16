import { classNames } from "@navtrack/shared/utils/tailwind";
import { NavLink } from "react-router-dom";
import { BadgeFlatPill } from "../../badge/BadgeFlatPill";

type TabsLayoutProps = {
  tabs: TabMenuItemProps[];
  children?: React.ReactNode;
};

export type TabMenuItemProps = {
  name: string;
  href: string;
  count?: number;
};

function TabMenuItem(props: TabMenuItemProps) {
  return (
    <NavLink
      key={props.name}
      to={props.href}
      end
      className={(props) =>
        classNames(
          props.isActive
            ? "border-blue-500 text-blue-600"
            : "border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700",
          "whitespace-nowrap border-b-2 px-1 py-4 text-sm font-medium"
        )
      }>
      {props.name}
      {props.count !== undefined && (
        <BadgeFlatPill className="ml-2" color="darkGray">
          {props.count}
        </BadgeFlatPill>
      )}
    </NavLink>
  );
}

export function TabsLayout(props: TabsLayoutProps) {
  return (
    <div className="flex items-center border-b border-gray-200">
      <nav className="flex flex-1 space-x-8">
        {props.tabs.map((tab) => (
          <TabMenuItem key={tab.name} {...tab} />
        ))}
      </nav>
    </div>
  );
}
