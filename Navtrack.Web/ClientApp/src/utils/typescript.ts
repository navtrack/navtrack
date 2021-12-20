export function nameOf<T>(key: keyof T): keyof T {
  return key;
}
