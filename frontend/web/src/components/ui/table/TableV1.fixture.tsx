import { TableV1 } from "./TableV1";
import { ITableColumn } from "./useTable";

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
  { labelId: "generic.email", row: (user) => user.email },
  { labelId: "generic.role", row: (user) => user.role }
];

export default {
  Default: <TableV1 columns={columns} rows={users} />,
  Loading: <TableV1 columns={columns} rows={undefined} />,
  "No Items": <TableV1 columns={columns} rows={[]} />
};
