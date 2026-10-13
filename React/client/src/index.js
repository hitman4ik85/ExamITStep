import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import 'antd/dist/reset.css';
import reportWebVitals from './reportWebVitals';
import { Provider } from 'react-redux';
import store from './store/store';
import { RouterProvider } from 'react-router-dom';
import routes from './routes/routes';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  //<React.StrictMode>
  <Provider store={store}>
    <RouterProvider router={routes} />
  </Provider>
  //</React.StrictMode>
);

reportWebVitals();
