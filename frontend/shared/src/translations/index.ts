import en from "./en.json";
import countryList from "./countries.json";

export const translations = {
  en: en
};

export const countries = countryList;

export const getCountryName = (code?: string) =>
  `${countries.find((x) => x.code === code)?.name}`;
