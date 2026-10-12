import { TableV1 } from "./TableV1";
import { ITableColumn, useTable } from "./useTable";

type User = {
  email: string;
  role: string;
};

const users: User[] = [
  {
    email: "user1@codeagency.com",
    role: "Owner"
  },
  {
    email: "user2@codeagency.com",
    role: "Viewer"
  },
  {
    email: "user3@codeagency.com",
    role: "Viewer"
  }
];

const columns: ITableColumn<User>[] = [
  { labelId: "email", row: (user) => user.email },
  { labelId: "role", row: (user) => user.role }
];

export default {
  Default: () => {
    const table = useTable({ columns, rows: users });

    return <TableV1 {...table.props} />;
  },
  Loading: () => {
    const table = useTable({ columns, rows: undefined });

    return <TableV1 {...table.props} />;
  },
  "No Items": () => {
    const table = useTable({ columns, rows: [] });

    return <TableV1 {...table.props} />;
  }
};
