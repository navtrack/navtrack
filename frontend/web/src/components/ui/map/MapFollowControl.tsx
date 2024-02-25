import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { useEffect, useState } from "react";
import { Icon } from "../icon/Icon";
import { faLocationArrow } from "@fortawesome/free-solid-svg-icons";
import { LongLat } from "./mapTypes";
import { MapCenter } from "./MapCenter";

type MapFollowControlProps = {
  position?: LongLat;
  follow?: boolean;
  onChange?: (follow: boolean) => void;
};

export function MapFollowControl(props: MapFollowControlProps) {
  const [follow, setFollow] = useState(
    props.follow !== undefined ? props.follow : true
  );

  useEffect(() => {
    if (props.follow !== undefined && props.follow !== follow) {
      setFollow(props.follow);
    }
  }, [follow, props.follow]);

  return (
    <div className="absolute bottom-0 z-10 mx-auto flex w-full justify-center">
      <div
        onClick={() => {
          setFollow(!follow);
          props.onChange?.(!follow);
        }}
        className={classNames(
          "mb-2 cursor-pointer rounded-lg bg-white px-2 py-0.5 text-sm font-medium shadow-md",
          c(follow, "text-blue-600", "text-gray-400")
        )}>
        <Icon icon={faLocationArrow} className="mr-1" />
        {follow ? "Follow On" : "Follow Off"}
      </div>
      {follow ? <MapCenter position={props.position} /> : null}
    </div>
  );
}
