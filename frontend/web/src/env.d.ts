/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly DEV: boolean;
  readonly VITE_API_URL: string;
  readonly VITE_VERSION: string;
  readonly VITE_LISTENER_HOSTNAME: string;
  readonly VITE_LISTENER_IP: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
