import { useField } from "formik";

type Props = {
  name?: string; // TODO: make mandatory
  error?: any;
};

export default function InputError(props: Props) {
  const [, meta] = useField(props.name ? props.name : "");

  return (
    <>
      {meta && meta.error && meta.touched && (
        <p className="text-red-600 text-xs italic mt-1">{meta.error}</p>
      )}
    </>
  );
}
