import React, { useState, useEffect } from 'react';
import axios from 'axios';
import '../../css/forms.css';

const ViewQuote = ({ onCancel, quoteId }) => {
    const [quoteTitle, setQuoteTitle] = useState('');
    const [clientName, setClientName] = useState('');
    const [quoteDate, setQuoteDate] = useState(null);

    useEffect(() => {
        const fetchQuote = async () => {
            try {
                const response = await axios.get(`${process.env.REACT_APP_API_URL}/quotes/getquotebyid`, {
                    params: { quoteId }
                });
                setQuoteTitle(response.data.quoteTitle);
                setClientName(response.data.client.firstName + ' ' + response.data.client.lastName);
                setQuoteDate(formatDate(new Date(response.data.quoteDate)));
            } catch (error) {
                console.error('Error fetching quote:', error);
            }
        };
        fetchQuote();
    }, [quoteId]);

    const formatDate = (date) => {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0'); // Months are zero-based, so add 1
        const day = String(date.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
    }

    return (
        <div className="card card-flush">
            <div className="card-header fw-bold">
                <div className="card-title">QUOTE DETAILS:</div>
                <div className="card-controls">
                </div>
            </div>
            <div className="card-body">
                <div className="form-container">
                    <div className="form-row">
                        <label>Quote Title:</label>
                        <input
                            type="text"
                            value={quoteTitle}
                            readOnly
                        />
                    </div>
                    <div className="form-row">
                        <label>Client Name:</label>
                        <div><input
                            type="text"
                            value={clientName}
                            readOnly
                        />
                        </div>
                    </div>
                    <div className="form-row">
                        <label>Quote Date:</label>
                        <div><input
                            type="date"
                            value={quoteDate}
                            readOnly
                        /></div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ViewQuote;
