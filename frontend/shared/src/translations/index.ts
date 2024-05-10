import en from "./en.json";
import countryList from "./countries.json";

export const translations = {
  en: en
};

export const countries = countryList;

const euCountryCodes = [
  "AT",
  "BE",
  "BG",
  "CY",
  "CZ",
  "DE",
  "DK",
  "EE",
  "EL",
  "ES",
  "FI",
  "FR",
  "HR",
  "HU",
  "IE",
  "IT",
  "LT",
  "LU",
  "LV",
  "MT",
  "NL",
  "PL",
  "PT",
  "RO",
  "SE",
  "SI",
  "SK"
];

export const euCountries = countries.filter((x) =>
  euCountryCodes.includes(x.code)
);

export const getCountryName = (code?: string) =>
  `${countries.find((x) => x.code === code)?.name}`;
