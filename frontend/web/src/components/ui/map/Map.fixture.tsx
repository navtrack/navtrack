import { useEffect, useState } from "react";
import { useFixtureInput } from "react-cosmos/client";
import { DEFAULT_MAP_CENTER } from "@navtrack/shared/constants";
import { Map } from "./Map";
import { MapFollowControl } from "./MapFollowControl";
import { MapPin } from "./MapPin";

const coordinates = [
  [46.772771, 23.594181],
  [46.770933, 23.602395],
  [46.773726, 23.596048],
  [46.771933, 23.61173],
  [46.771734, 23.598932],
  [46.777613, 23.603597],
  [46.772532, 23.605141],
  [46.776595, 23.607795],
  [46.776812, 23.599106],
  [46.775059, 23.6103],
  [46.770002, 23.604589],
  [46.769776, 23.601557],
  [46.770135, 23.596103],
  [46.767577, 23.611235],
  [46.767959, 23.590313],
  [46.771999, 23.612894],
  [46.770973, 23.604092],
  [46.766682, 23.61093],
  [46.769098, 23.611976],
  [46.770126, 23.595498],
  [46.773647, 23.610678],
  [46.7752, 23.601565],
  [46.76953, 23.605654],
  [46.77026, 23.606524],
  [46.767366, 23.593498],
  [46.773769, 23.615168],
  [46.773932, 23.608244],
  [46.768418, 23.613371],
  [46.777931, 23.605411],
  [46.770259, 23.602584],
  [46.774513, 23.608362],
  [46.767758, 23.601681],
  [46.770249, 23.608878],
  [46.770361, 23.607063],
  [46.773698, 23.590424],
  [46.767471, 23.610009],
  [46.76657, 23.613007],
  [46.766302, 23.612851],
  [46.774756, 23.593491],
  [46.767176, 23.613897],
  [46.768313, 23.599736],
  [46.772106, 23.604715],
  [46.770431, 23.595568],
  [46.774433, 23.596531],
  [46.769816, 23.601023],
  [46.769307, 23.61199],
  [46.773556, 23.605203],
  [46.777789, 23.594193],
  [46.771854, 23.602098],
  [46.770345, 23.597319],
  [46.776606, 23.590307],
  [46.770422, 23.609732],
  [46.772027, 23.589649],
  [46.770151, 23.601274],
  [46.776538, 23.589648],
  [46.772379, 23.609629],
  [46.768446, 23.594145],
  [46.775258, 23.599534],
  [46.775053, 23.602515],
  [46.768062, 23.597811],
  [46.770889, 23.605865],
  [46.777925, 23.599708],
  [46.769981, 23.611518],
  [46.774777, 23.611225],
  [46.768249, 23.614933],
  [46.766351, 23.594579],
  [46.769671, 23.591505],
  [46.771087, 23.59136],
  [46.766235, 23.611041],
  [46.777851, 23.611328],
  [46.773392, 23.594599],
  [46.777769, 23.611522],
  [46.76986, 23.611944],
  [46.772189, 23.604208],
  [46.771325, 23.603135],
  [46.768985, 23.594406],
  [46.772011, 23.605066],
  [46.767542, 23.599707],
  [46.774086, 23.615345],
  [46.773707, 23.603042],
  [46.769284, 23.599082],
  [46.774073, 23.59164],
  [46.77789, 23.615436],
  [46.770292, 23.616173],
  [46.768718, 23.595952],
  [46.775868, 23.6117],
  [46.771626, 23.596776],
  [46.775144, 23.60834],
  [46.777334, 23.610767],
  [46.77228, 23.611973],
  [46.777022, 23.598907],
  [46.767643, 23.611005],
  [46.766941, 23.597945],
  [46.771873, 23.591642],
  [46.769455, 23.605177],
  [46.774493, 23.613566],
  [46.777161, 23.595152],
  [46.767358, 23.594371],
  [46.770331, 23.606118]
];

export default {
  Default: () => {
    return <Map center={DEFAULT_MAP_CENTER} />;
  },
  WithPin: () => {
    const [latitude] = useFixtureInput("latitude", 46.770439);
    const [longitude] = useFixtureInput("longitude", 23.591423);

    return (
      <Map center={DEFAULT_MAP_CENTER}>
        <MapPin
          pin={{
            coordinates: { latitude: latitude, longitude: longitude },
            follow: true
          }}
        />
      </Map>
    );
  },
  WithFollow: () => {
    const [latitude, setLatitude] = useFixtureInput("latitude", 46.770439);
    const [longitude, setLongitude] = useFixtureInput("longitude", 23.591423);
    const [follow, setFollow] = useFixtureInput("follow", true);

    const [interval, setLocalInterval] = useState<NodeJS.Timeout>();

    useEffect(() => {
      if (interval === undefined) {
        const intervalId = setInterval(() => {
          const index = Math.floor(Math.random() * coordinates.length);
          const [latitude, longitude] = coordinates[index];
          setLatitude(latitude);
          setLongitude(longitude);
        }, 2000);

        setLocalInterval(intervalId);

        return () => clearInterval(interval);
      }
    }, [follow, interval, setLatitude, setLongitude]);

    return (
      <Map center={DEFAULT_MAP_CENTER}>
        <MapPin
          pin={{ coordinates: { latitude: latitude, longitude: longitude } }}
        />
        <MapFollowControl
          position={{ latitude: latitude, longitude: longitude }}
          follow={follow}
          onChange={setFollow}
        />
      </Map>
    );
  }
};
