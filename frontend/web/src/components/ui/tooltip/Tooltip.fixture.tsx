import { Tooltip } from "./Tooltip";

export default {
  Default: () => {
    return (
      <div className="p-8">
        <Tooltip content="This is a tooltip content">
          <div className="bg-red-100">this is a div for test</div>
        </Tooltip>
      </div>
    );
  }
};
