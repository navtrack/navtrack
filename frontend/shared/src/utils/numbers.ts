export function isNumeric(value?: string | number | null): boolean {
  return value !== undefined && value !== null && !isNaN(+value);
}

export function randomInteger(min: number, max: number) {
  return Math.floor(Math.random() * (max - min + 1)) + min;
}
