import { Outlet } from 'react-router-dom';
import Header from "./components/Header/Header";
import { useEffect } from "react";
import { getAuthUser } from "./thunks/authThunk";
import { useDispatch, useSelector } from "react-redux";
import { getToken } from "./utils/token";
import Loader from "./components/Loader/Loader";

const App = () => {
  const dispatch = useDispatch();
  const auth = useSelector((state) => state.auth);

  useEffect(() => {
    if (getToken())
      dispatch(getAuthUser());
  }, [dispatch]);

  return (
    <div>
      {auth.status === "loading" && <Loader />}

      <Header />
      <div style={{ padding: "0 40px" }}>
        <Outlet />
      </div>
    </div>
  );
};

export default App;
