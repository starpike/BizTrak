import React, { useState, useEffect } from 'react';
import Pagination from '../Pagination'
import { formatDate } from '../utilities/formatting';
import axios from 'axios';
import PropTypes from 'prop-types';

function QuotesList({ onAdd, onSelect }) {
    const [quotes, setQuotes] = useState([]);
    const [error, setError] = useState(null);
    const [totalCount, setTotalCount] = useState(0);
    const [page, setPage] = useState(1);
    const [pageSize] = useState(10);
    const [search, setSearch] = useState('');

    useEffect(() => {
        const fetchQuotes = async () => {
            try {
                const response = await axios.get(`${process.env.REACT_APP_API_URL}/quotes/search`, {
                    params: { page, pageSize, search }
                });
                setQuotes(response.data.quotes);
                setTotalCount(response.data.total);
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
        if (event.target.value.length === 0 || event.target.value.length > 1) {
            setSearch(event.target.value);
            setPage(1);
        }
    };

    QuotesList.propTypes = {
        onAdd: PropTypes.func.isRequired,
        onSelect: PropTypes.func.isRequired
      }

    return (
        <div className="card card-flush">
            <div className="card-header fw-bold">
                <div className="card-title">QUOTES:</div>
                <div className="card-controls">
                    <input type="text" className='mlr-1' placeholder='search quotes...' onChange={handleSearchChange} />
                    <button type="button" className="btn" onClick={onAdd}>Add Quote</button>
                </div>
            </div>
            <div className="card-body">
                <table className="responsive-table">
                    <thead>
                        <tr>
                            <th>Ref</th>
                            <th>Title</th>
                            <th>Name</th>
                            <th>Address</th>
                            <th>Date</th>
                            <th>Status</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        {quotes.map((quote, index) => (
                            <tr key={index}>
                                <td data-label="Quote Ref:" className='nowrap'><button className="button-as-link" onClick={() => onSelect(quote.id)}>{quote.quoteRef.toUpperCase()}</button></td>
                                <td data-label="Quote Title:">{quote.quoteTitle}</td>
                                <td data-label="Client Name:">{quote.client.firstName + ' ' + quote.client.lastName}</td>
                                <td data-label="Client Address:">{quote.client.addressLine1}</td>
                                <td data-label="Date:">{formatDate(quote.quoteDate)}</td>
                                <td data-label="Status:">{quote.statusName}</td>
                                <td data-label="Action:">
                                    <button className="ellipsis-button">
                                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                            <circle cx="5" cy="12" r="2" fill="black" />
                                            <circle cx="12" cy="12" r="2" fill="black" />
                                            <circle cx="19" cy="12" r="2" fill="black" />
                                        </svg>
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
                <Pagination page={page} setPage={setPage} totalCount={totalCount} pageSize={pageSize} />
            </div>
        </div>
    );
}

export default QuotesList;