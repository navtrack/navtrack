export const getParameterCaseInsensitive = (object: any, key: string) => {
  if (object) {
    const key2: string | undefined = Object.keys(object).find(k => k.toLowerCase() === key.toLowerCase());

    if (key2) {
      return object[key2];
    }
  }

  return undefined;
};