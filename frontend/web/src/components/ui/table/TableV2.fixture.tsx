import { TableV2 } from "./TableV2";
import { ITableColumn, useTable } from "./useTable";

type User = {
  email: string;
  role: string;
};

const users: User[] = Array.from({ length: 1000 }, (_, i) => ({
  email: `user${i}@codeagency.com`,
  role: "Viewer"
}));

const columns: ITableColumn<User>[] = [
  {
    labelId: "email",
    rowClassName: "py-1",
    value: (user) => user.email,
    row: (user) => user.email
  },
  {
    labelId: "role",
    rowClassName: "py-1",
    value: (user) => user.role,
    row: (user) => user.role
  }
];

export default {
  Default: () => {
    const table = useTable({ columns, rows: users });

    return <TableV2 className="h-80" {...table.props} />;
  },
  "With footer": () => {
    const columnsWithFooter = columns.map((column, i) => ({
      ...column,
      footer: () => `Footer ${i + 1}`
    }));
    const table = useTable({ columns: columnsWithFooter, rows: users });

    return <TableV2 className="h-80" {...table.props} />;
  },
  Loading: () => {
    const table = useTable({ columns, rows: undefined });

    return <TableV2 {...table.props} />;
  },
  "No Items": () => {
    const table = useTable({ columns, rows: [] });

    return <TableV2 {...table.props} />;
  },
  "With Selected Item": () => {
    const table = useTable({
      columns,
      rows: users,
      selection: true,
      equals: (a, b) => a.email === b.email
    });

    return <TableV2 className="h-80" {...table.props} />;
  }
};
