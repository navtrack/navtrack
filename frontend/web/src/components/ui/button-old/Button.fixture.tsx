import { LoadingIndicator } from "../loading-indicator/LoadingIndicator";
import { Button } from "./Button";

export default {
  Primary: (
    <div className="block space-x-4">
      <Button size="xs">Click me</Button>
      <Button size="sm">Click me</Button>
      <Button size="md">Click me</Button>
      <Button>Click me</Button>
      <Button size="lg">Click me</Button>
    </div>
  ),
  PrimaryLoading: (
    <div className="block space-x-4">
      <Button size="xs">Click me</Button>
      <Button size="xs">
        <LoadingIndicator />
      </Button>
      <Button size="sm">Click me</Button>
      <Button size="sm">
        <LoadingIndicator />
      </Button>
      <Button size="md">Click me</Button>
      <Button size="md">
        <LoadingIndicator />
      </Button>
      <Button>Click me</Button>
      <Button>
        <LoadingIndicator />
      </Button>
      <Button size="lg">Click me</Button>
      <Button size="lg">
        <LoadingIndicator />
      </Button>
    </div>
  ),
  Secondary: (
    <div className="space-x-4">
      <Button size="sm" color="secondary">
        Click me
      </Button>
      <Button size="md" color="secondary">
        Click me
      </Button>
      <Button color="secondary">Click me</Button>
    </div>
  ),
  White: (
    <div className="space-x-4">
      <Button size="sm" color="white">
        Click me
      </Button>
      <Button size="md" color="white">
        Click me
      </Button>
      <Button color="white">Click me</Button>
    </div>
  ),
  Green: (
    <div className="space-x-4">
      <Button size="sm" color="green">
        Click me
      </Button>
      <Button size="md" color="green">
        Click me
      </Button>
      <Button color="green">Click me</Button>
    </div>
  ),
  Warn: (
    <div className="space-x-4">
      <Button size="sm" color="warn">
        Click me
      </Button>
      <Button size="md" color="warn">
        Click me
      </Button>
      <Button color="warn">Click me</Button>
    </div>
  )
};
