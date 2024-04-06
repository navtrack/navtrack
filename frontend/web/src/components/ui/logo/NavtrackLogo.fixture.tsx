import { NavtrackLogo } from "./NavtrackLogo";
import { NavtrackLogoDark } from "./NavtrackLogoDark";

export default {
  Default: () => {
    return (
      <div
        className="flex items-center justify-center bg-gray-900"
        style={{
          width: 1024,
          height: 512
        }}>
        <div style={{ width: 450 }}>
          <NavtrackLogo />
        </div>
      </div>
    );
  },
  Branding: () => {
    return (
      <div
        className="flex items-center justify-center bg-gray-900"
        style={{
          width: 1024,
          height: 500
        }}>
        <div style={{ width: 300 }}>
          <NavtrackLogo />
        </div>
        <div
          className="ml-16 font-semibold text-white"
          style={{
            fontSize: 130
          }}>
          Navtrack
        </div>
      </div>
    );
  }
};
