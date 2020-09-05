export const containsOnlyDigits = (object: any) => {
  return /^\d+$/.test(object);
};

export const getRangeArray = (start: number, end: number) => {
  let array = [];

  for (let i = start; i <= end; i++) {
    array.push(i);
  }

  return array;
};