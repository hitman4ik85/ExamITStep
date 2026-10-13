import { Navigate } from 'react-router-dom';
import useAuth from '../hooks/useAuth';

const AuthRoute = ({ children }) => {
    const isAuth = useAuth();
    return isAuth ? children : <Navigate to = "/login" />;
}

export default AuthRoute;
