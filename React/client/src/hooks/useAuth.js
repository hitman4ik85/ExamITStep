import { useSelector } from "react-redux";

const useAuth = () => {
    const token = useSelector((state) => state.auth.token);
    return token !== null ? true : false;
};

export default useAuth;