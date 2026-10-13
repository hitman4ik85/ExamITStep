// Це маршрутизатор для React додатку, який використовує React Router для навігації між компонентами "Customer" та "Seller".
import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Customer from './Customer';
import Seller from './Seller';

function Home() {
  return (
    <div className="min-vh-100 bg-gradient-calm text-on-dark d-flex flex-column">
      <h4 className="mb-2">Internet Shop</h4>
      <p>Оберіть: "Seller"(продавець) або "Customer"(покупець), для навігації</p>
    </div>
  );
}

export default function AppRouter() {
  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/seller" element={<Seller />} />
      <Route path="/customer" element={<Customer />} />
    </Routes>
  );
}
