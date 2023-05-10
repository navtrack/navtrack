import { ErrorMessage } from "@navtrack/shared/components/ErrorMessage";

export interface InputErrorProps {
  error?: string;
}
export const InputError = (props: InputErrorProps) => {
  return (
    <>
      {props.error && (
        <p className="mt-1 text-xs text-red-600">
          <ErrorMessage code={props.error} />
        </p>
      )}
    </>
  );
};
