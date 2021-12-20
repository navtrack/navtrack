export type ConfigState = {
  config?: Config;
  loading?: boolean;
};

export type Config = {
  apiUrl: string;
  mapTileUrl: string;
  sentryDsn?: string;
  environment?: string;
};
