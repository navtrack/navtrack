export function isNumeric(value: string) {
  return !isNaN(+value);
}

export function randomInteger(min: number, max: number) {
  return Math.floor(Math.random() * (max - min + 1)) + min;
}
