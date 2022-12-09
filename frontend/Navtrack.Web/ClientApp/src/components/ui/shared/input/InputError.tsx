export interface IInputError {
  error?: string;
}
export default function InputError(props: IInputError) {
  return (
    <>
      {props.error && (
        <p className="text-red-600 text-xs mt-1">{props.error}</p>
      )}
    </>
  );
}
