import Table, { ITableColumn } from "./Table";

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
  { labelId: "generic.email", render: (user) => user.email },
  { labelId: "generic.role", render: (user) => user.role }
];

export default {
  Default: () => {
    return <Table columns={columns} rows={users} />;
  },
  Loading: () => {
    return <Table columns={columns} rows={[]} loading />;
  },
  "No Items": () => {
    return <Table columns={columns} rows={[]} />;
  }
};
