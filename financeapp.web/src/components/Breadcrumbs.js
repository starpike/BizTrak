import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import '../css/breadcrumbs.css'; // Importing the CSS for styling

const Breadcrumbs = () => {
  const location = useLocation();
  const pathnames = location.pathname.split('/').filter(x => x);

  return (
    <div className="breadcrumb">
      <Link to="/" className="breadcrumb-item">Home</Link>
      {pathnames.length > 0 && <span className="breadcrumb-separator">&#9654;</span>}
      {pathnames.map((value, index) => {
        const last = index === pathnames.length - 1;
        const to = `/${pathnames.slice(0, index + 1).join('/')}`;
        return last ? (
          <span key={to} className="breadcrumb-item active">{value}</span>
        ) : (
          <>
            <Link key={to} to={to} className="breadcrumb-item">{value}</Link>
            <span className="breadcrumb-separator">›</span>
          </>
        );
      })}
    </div>
  );
};

export default Breadcrumbs;
