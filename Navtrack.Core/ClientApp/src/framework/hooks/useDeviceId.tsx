import { useParams } from "react-router";

const useDeviceId = (): number => {
  let { deviceId } = useParams<{ deviceId: string }>();

  return deviceId ? parseInt(deviceId) : 0;
};

export default useDeviceId;
