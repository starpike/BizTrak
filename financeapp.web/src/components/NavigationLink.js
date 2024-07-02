import React from 'react';
import { NavLink, useLocation } from 'react-router-dom';

const NavigationLink = ({ to, label }) => {
    const location = useLocation();
    const isActive = location.pathname === to;
    return (
        <li className="nav-item">
            <NavLink className={`nav-link ${isActive ? 'active' : ''}`} to={to}>
                {label}
            </NavLink>
        </li>
    );
};

export default NavigationLink;