export type AppConfig = {
  api: {
    url: string;
  };
  reactQueryDevtools?: boolean;
  authentication: {
    clientId: string;
    apple?: {
      clientId: string;
      redirectUri: string;
    };
    google?: {
      clientId: string;
      redirectUri?: string;
    };
    microsoft?: {
      clientId: string;
      authority: string;
      redirectUri?: string;
    };
  };
  map: {
    tileUrl: string;
  };
  captcha?: {
    siteKey: string;
  };
};

class AppConfigStore {
  public config: AppConfig = {
    api: {
      url: ""
    },
    authentication: {
      clientId: ""
    },
    map: {
      tileUrl: ""
    }
  };
  public initialized = false;
}

export const appConfigStore = new AppConfigStore();
