import { ErrorMessage } from "@navtrack/shared/components/ui/ErrorMessage";

export type InputErrorProps = {
  error?: string;
};

export function InputError(props: InputErrorProps) {
  return (
    <>
      {props.error && (
        <p className="mt-1 text-xs text-red-600">
          <ErrorMessage code={props.error} />
        </p>
      )}
    </>
  );
}
