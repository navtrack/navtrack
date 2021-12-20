import useAppContext from "../hooks/app/useAppContext";

const AppContextProvider: React.FC = (props) => {
  useAppContext();

  return <>{props.children}</>;
};

export default AppContextProvider;
