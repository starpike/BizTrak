import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, Link, Navigate } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
  return (
    <Router>
      <div className="container">
        <Header />
        <Navigation />
        <Routes>
          <Route path="/" element={<Navigate to="/quotes" replace />} />
          <Route path="/quotes" element={<Quotes />} />
        </Routes>
      </div>
    </Router>
  );
}

function Header() {
  return (
    <header className="py-3 mb-3 border-bottom">
      <div className="d-flex flex-column flex-md-row align-items-center pb-1 mb-1">
        <a href="/" className="d-flex align-items-center text-dark text-decoration-none">
          <span className="fs-2">Business Tracker</span>
        </a>
      </div>
    </header>
  );
}

function Navigation() {
  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-light">
      <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
        <span className="navbar-toggler-icon"></span>
      </button>
      <div className="collapse navbar-collapse" id="navbarNav">
        <ul className="navbar-nav">
          <li className="nav-item">
            <Link className="nav-link" to="/quotes">Quotes</Link>
          </li>
          <li className="nav-item">
            <Link className="nav-link" to="/invoices">Invoices</Link>
          </li>
          <li className="nav-item">
            <Link className="nav-link" to="/clients">Clients</Link>
          </li>
          <li className="nav-item">
            <Link className="nav-link" to="/expenses">Expenses</Link>
          </li>
        </ul>
      </div>
    </nav>
  );
}

function Quotes() {
  const [quotes, setQuotes] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    const apiUrl = `${process.env.REACT_APP_API_URL}/quotes`;

    fetch(apiUrl)
      .then(response => response.json())
      .then(data => setQuotes(data))
      .catch(error => {
        console.error('Error fetching quotes:', error);
        setError('Failed to load quotes');
      });
  }, []);

  if (error) {
    return <div>Error: {error}</div>;
  }

  const formatDate = (dateString) => {
    const date = new Date(dateString);
    return `${date.getDate().toString().padStart(2, '0')}/${(date.getMonth() + 1).toString().padStart(2, '0')}/${date.getFullYear()}`;
  };

  return (
    <div className="container">
      <div className="row bg-info">
        <div className="col text-left p-2"><strong>Quotes:</strong></div>
      </div>
      <div className="row bg-dark text-white">
        <div className="col text-left p-2"><strong>Ref</strong></div>
        <div className="col text-left p-2"><strong>Quote Title</strong></div>
        <div className="col text-left p-2"><strong>Date</strong></div>
      </div>
      {quotes.map((quote, index) => (
        <div key={index} className={`row ${index % 2 === 0 ? 'bg-light' : ''}`}>
          <div className="col text-left p-2">{quote.quoteRef.toUpperCase()}</div>
          <div className="col text-left p-2">{quote.quoteTitle}</div>
          <div className="col text-left p-2">{formatDate(quote.quoteDate)}</div>
        </div>
      ))}
    </div>
  );
}

export default App;
