import './App.css';
import "bootstrap/dist/css/bootstrap.min.css";
import { BrowserRouter, Link } from 'react-router-dom';
import AppRouter from './components/AppRouter';

function App() {
  return (
    <BrowserRouter>
      <div className="App">
        <nav className="nav bg-warning topnav" data-bs-theme="dark">
          <Link className="nav-link" to="/">ShopContent</Link>
          <Link className="nav-link" to="/seller">Seller</Link>
          <Link className="nav-link" to="/customer">Customer</Link>
        </nav>
        <AppRouter />
      </div>
    </BrowserRouter>
  );
}

export default App;
