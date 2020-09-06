export type DeviceConnectionResponseModel = {
  id: number;
  deviceId: number;
  openedAt: Date;
  closedAt?: Date
  remoteEndPoint: string
  messages: number
};