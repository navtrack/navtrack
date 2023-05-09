import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { ReactNode } from "react";
import { FormattedMessage } from "react-intl";
import { Button } from "../../../ui/shared/button/Button";
import { Icon } from "../../../ui/shared/icon/Icon";

interface IFilterModal {
  icon: IconProp;
  children?: ReactNode;
  className?: string;
  onCancel: () => void;
}

export function FilterModal(props: IFilterModal) {
  return (
    <div className={props.className}>
      <div className="flex flex-grow">
        <div className="p-4">
          <div className="mx-auto flex h-12 w-12 flex-shrink-0 items-center justify-center rounded-full bg-gray-900 text-white sm:mx-0 sm:h-10 sm:w-10">
            <Icon icon={props.icon} />
          </div>
        </div>
        <div className="flex flex-grow flex-col p-4 pl-0">{props.children}</div>
      </div>
      <div className="flex flex-row-reverse space-x-4 space-x-reverse rounded-b-lg bg-gray-50 px-4 py-3">
        <Button color="primary" type="submit">
          <FormattedMessage id="generic.save" />
        </Button>
        <Button color="white" onClick={props.onCancel}>
          <FormattedMessage id="generic.cancel" />
        </Button>
      </div>
    </div>
  );
}
