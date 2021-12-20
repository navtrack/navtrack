import { useAxiosBaseUrls } from "../api/useAxiosBaseUrls";
import useAxiosAuthorization from "../api/useAxiosAuthorization";

const AxiosConfigurationProvider: React.FC = (props) => {
  useAxiosBaseUrls();
  useAxiosAuthorization();

  return <>{props.children}</>;
};

export default AxiosConfigurationProvider;
