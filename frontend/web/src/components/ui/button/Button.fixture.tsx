import { faPlus } from "@fortawesome/free-solid-svg-icons";
import { Button } from "./Button";

export default {
  Size: (
    <div className="block space-x-4">
      <Button size="xs">Click me</Button>
      <Button size="sm">Click me</Button>
      <Button>Click me</Button>
      <Button size="md">Click me</Button>
      <Button size="lg">Click me</Button>
    </div>
  ),
  Color: (
    <div className="block space-x-4">
      <Button color="primary">Click me</Button>
      <Button color="secondary">Click me</Button>
      <Button color="white">Click me</Button>
      <Button color="success">Click me</Button>
      <Button color="error">Click me</Button>
    </div>
  ),
  Disabled: (
    <div className="block space-x-4">
      <Button color="primary" disabled>
        Click me
      </Button>
      <Button color="secondary" disabled>
        Click me
      </Button>
      <Button color="white" disabled>
        Click me
      </Button>
      <Button color="success" disabled>
        Click me
      </Button>
      <Button color="error" disabled>
        Click me
      </Button>
    </div>
  ),
  Loading: (
    <div className="block space-x-4">
      <Button color="primary" isLoading>
        Click me
      </Button>
      <Button color="secondary" isLoading>
        Click me
      </Button>
      <Button color="white" isLoading>
        Click me
      </Button>
      <Button color="success" isLoading>
        Click me
      </Button>
      <Button color="error" isLoading>
        Click me
      </Button>
    </div>
  ),
  Icon: (
    <div className="block space-x-4">
      <Button color="primary" icon={faPlus}>
        Click me
      </Button>
      <Button color="secondary" icon={faPlus}>
        Click me
      </Button>
      <Button color="white" icon={faPlus}>
        Click me
      </Button>
      <Button color="success" icon={faPlus}>
        Click me
      </Button>
      <Button color="error" icon={faPlus}>
        Click me
      </Button>
    </div>
  )
};
