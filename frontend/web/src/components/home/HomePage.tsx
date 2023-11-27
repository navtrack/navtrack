import { Card } from "../ui/card/Card";
import { AuthenticatedLayoutTwoColumns } from "../ui/layouts/authenticated/AuthenticatedLayoutTwoColumns";

export function HomePage() {
  return (
    <AuthenticatedLayoutTwoColumns>
      <Card className="p-4">Welcome to Navtrack!</Card>
    </AuthenticatedLayoutTwoColumns>
  );
}
