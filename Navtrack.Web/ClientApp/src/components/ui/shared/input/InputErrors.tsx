import { FormikErrors } from "formik";
import InputError from "./InputError";

export interface IInputErrors<T> {
  errors?: FormikErrors<T>;
}

export default function InputErrors<T>(props: IInputErrors<T>) {
  return (
    <>
      {props.errors &&
        Object.values(props.errors).map((x) => (
          <InputError error={x as unknown as string} />
        ))}
    </>
  );
}
