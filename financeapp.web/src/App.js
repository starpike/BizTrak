import React, { useState, useRef, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, NavLink, useLocation } from 'react-router-dom';
import QuotesWrapper from './components/quotes/QuotesWrapper';
import Breadcrumbs from './components/Breadcrumbs';
import './css/app.css';
import './css/cards.css';
import './css/datagrid.css';
import TitleIcon from './logo.svg'; // Assuming SVG loader is configured

const Dashboard = () => (
    <div className="card card-flush">
        <div className="card-header">
        <div className="card-title">DASHBOARD:</div>
        </div>
        <div className="card-body pt-6">More content beyond the sidebar here.</div>
    </div>
);

const PageWrapper = ({ title, children }) => (
    <div>
        <div className="container-fluid">
            <Breadcrumbs />
            {children}
        </div>
    </div>
);

function Header({ title }) {
    return (
        <div className="bg-white py-2 px-4">
            <span className="fs-6">{title}</span>
        </div>
    );
};

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

const App = () => {
    const [isNavCollapsed, setIsNavCollapsed] = useState(true);
    const sidebarRef = useRef(null);
    const togglerRef = useRef(null);

    const handleNavCollapse = () => {
        document.body.style.overflow = isNavCollapsed ? 'hidden' : '';
        setIsNavCollapsed(!isNavCollapsed);
    }

    useEffect(() => {
        const handleClickOutside = (event) => {
            if (!sidebarRef.current?.contains(event.target) && !togglerRef.current?.contains(event.target)) {
                setIsNavCollapsed(true);
                document.body.style.overflow = '';
            }
        };

        const handleResize = () => {
            setIsNavCollapsed(true);
            document.body.style.overflow = '';
        };

        document.addEventListener('mousedown', handleClickOutside);
        window.addEventListener('resize', handleResize);

        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
            window.removeEventListener('resize', handleResize);
        };
    }, []);

    return (
        <Router>
            <div className="container-fluid">
                <div className="row">
                    <nav ref={sidebarRef} className={`side-bar bg-side-bar display-med-block ${!isNavCollapsed ? 'show-side-bar' : ''}`}>
                        <div className="pt-3">
                            <NavLink to="/" className="center-flex text-decoration-none">
                                <img src={TitleIcon} className="h-20px p-3" />
                            </NavLink>
                            <hr />
                            <ul className="nav nav-pills flex-column">
                                <NavigationLink to="/" label="Dashboard" />
                                <NavigationLink to="/quotes" label="Quotes" />
                                <NavigationLink to="/invoices" label="Invoices" />
                                <NavigationLink to="/clients" label="Clients" />
                                <NavigationLink to="/expenses" label="Expenses" />
                            </ul>
                        </div>
                    </nav>
                    <main className="col-auto">
                        <div className="flex-box flex-box-h-align-end navbar-toggler ">
                            <button ref={togglerRef} onClick={handleNavCollapse} className="p-0 ml-3 display-med-none">
                                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 30 30" width="30" height="30">
                                    <title>Menu</title>
                                    <path stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeMiterlimit="10" d="M4 7h22M4 15h22M4 23h22"></path>
                                </svg>
                            </button>
                        </div>
                        <div className="plr-6">
                            <Routes>
                                <Route path="/" element={<PageWrapper title="Dashboard"><Dashboard /></PageWrapper>} />
                                <Route path="/quotes" element={<PageWrapper title="Quotes"><QuotesWrapper /></PageWrapper>} />
                                <Route path="/invoices" element={<PageWrapper title="Invoices" />} />
                                <Route path="/clients" element={<PageWrapper title="Clients" />} />
                                <Route path="/expenses" element={<PageWrapper title="Expenses" />} />
                            </Routes>
                        </div>
                    </main>
                </div>
            </div>
        </Router>
    );
};

export default App;
