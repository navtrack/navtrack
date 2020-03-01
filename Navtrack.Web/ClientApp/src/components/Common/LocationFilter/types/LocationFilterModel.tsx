import { DateFilterModel, DefaultDateFilterModel } from "./DateFilterModel";
import { SpeedFilterModel, DefaultSpeedFilterModel } from "./SpeedFilterModel";
import { AltitudeFilterModel, DefaultAltitudeFilterModel } from "./AltitudeFilterModel";
import { CoordinatesFilterModel, DefaultCoordinatesFilterModel } from "./CoordinatesFilterModel";

export type LocationFilterModel = {
  date: DateFilterModel;
  speed: SpeedFilterModel;
  altitude: AltitudeFilterModel;
  coordinates: CoordinatesFilterModel;
};

export const DefaultLocationFilterModel: LocationFilterModel = {
  date: DefaultDateFilterModel,
  speed: DefaultSpeedFilterModel,
  altitude: DefaultAltitudeFilterModel,
  coordinates: DefaultCoordinatesFilterModel
};
