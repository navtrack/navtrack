import useSentry from "../hooks/sentry/useSentry";

const SentryProvider: React.FC = (props) => {
  useSentry();

  return <>{props.children}</>;
};

export default SentryProvider;
