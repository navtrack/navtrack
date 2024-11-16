export function nameOf<T1, T2 = T1>(
  key1: keyof T1,
  key2: keyof T2 | undefined = undefined
): string {
  if (key2 === undefined) {
    return String(key1);
  }
  return `${String(key1)}.${String(key2)}`;
}
