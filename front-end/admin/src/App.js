import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import AdminLogin from './components/AdminLogin';
import ProductList from './components/ProductList';

function App() {
  return (
    <Router>
      <Routes>
      <Route path="/" element={<AdminLogin />} />
      <Route path="/login" element={<AdminLogin />} />
      <Route path="/product" element={<ProductList />} />
      </Routes>
    </Router>
  );
}

export default App;
