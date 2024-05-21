import React, { useState, useEffect } from 'react';
import { NavLink } from 'react-router-dom';
import Pagination from './Pagination'
import axios from 'axios';
import '../css/cards.css';
import '../css/datagrid.css';

function Quotes() {
    const [quotes, setQuotes] = useState([]);
    const [error, setError] = useState(null);
    const [totalCount, setTotalCount] = useState(0);
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [search, setSearch] = useState('');

    useEffect(() => {
        const fetchQuotes = async () => {
            try {
                const response = await axios.get(`${process.env.REACT_APP_API_URL}/quotes/search`, {
                    params: { page, pageSize, search }
                });
                setQuotes(response.data.data);
                setTotalCount(response.data.totalCount);
            } catch (error) {
                console.error('Error fetching quotes:', error);
                setError('Failed to load quotes');
            }
        };
        fetchQuotes();
    }, [page, pageSize, search]);

    if (error) {
        return <div>Error: {error}</div>;
    }

    const handleSearchChange = (event) => {
        if (event.target.value.length == 0 ||event.target.value.length > 1) {
            setSearch(event.target.value);
            setPage(1);
        }
    };

    const formatDate = (dateString) => {
        const date = new Date(dateString);
        return `${date.getDate().toString().padStart(2, '0')}/${(date.getMonth() + 1).toString().padStart(2, '0')}/${date.getFullYear()}`;
    };

    return (
        <div className="card card-flush">
            <div className="card-header fw-bold">
                <div className="card-title">QUOTES:</div>
                <div className="card-controls">
                    <input type="text" className='mlr-1' placeholder='search quotes...' onChange={handleSearchChange} />
                    <button type="button" className="btn">Add Quote</button>
                </div>
            </div>
            <div className="card-body">
                <table className="responsive-table">
                    <thead>
                        <tr>
                            <th>Quote Ref</th>
                            <th>Quote Title</th>
                            <th>Client Name</th>
                            <th>Client Address</th>
                            <th>Date</th>
                        </tr>
                    </thead>
                    <tbody>
                        {quotes.map((quote, index) => (
                            <tr key={index}>
                                <td data-label="Quote Ref:"><NavLink to="/">{quote.quoteRef.toUpperCase()}</NavLink></td>
                                <td data-label="Quote Title:">{quote.quoteTitle}</td>
                                <td data-label="Client Name:">{quote.client.firstName + ' ' + quote.client.lastName}</td>
                                <td data-label="Client Address:">{quote.client.addressLine1}</td>
                                <td data-label="Date:">{formatDate(quote.quoteDate)}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
                <Pagination page={page} setPage={setPage} totalCount={totalCount} pageSize={pageSize} />
            </div>
        </div>
    );
}

export default Quotes;