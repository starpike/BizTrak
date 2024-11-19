import React, { useState, useEffect } from 'react';
import axios from 'axios';
import ClientLookup from '../clients/ClientLookup'; // Import the popup component
import '../../css/forms.css';
import PropTypes from 'prop-types';


const AddQuoteForm = ({ onSave, onCancel }) => {
    const [quoteTitle, setQuoteTitle] = useState('');
    const [clientId, setClientId] = useState(0);
    const [clientName, setClientName] = useState('');
    const [quoteDate, setQuoteDate] = useState(null);
    const [showPopup, setShowPopup] = useState(false);
    const [errors, setErrors] = useState([]);

    const handleSubmit = async (event) => {
        event.preventDefault();
        const newQuote = { quoteTitle, quoteDate, clientId };
        try {
            const response = await axios.post(`${process.env.REACT_APP_API_URL}/quotes`, newQuote);
            setErrors([]);
            onSave(response.data);
        } catch (error) {
            if (error.response && error.response.status === 400) {
                setErrors(error.response.data || []);
                console.log(errors);
            } else {
                console.error('An unexpected error occurred:', error);
            }
        }
    };

    AddQuoteForm.propTypes = {
        onSave: PropTypes.func.isRequired,
        onCancel: PropTypes.func.isRequired
      }

    useEffect(() => {
        if (showPopup) {
            document.body.style.overflow = 'hidden';
        } else {
            document.body.style.overflow = 'visible';
        }
    }, [showPopup]);

    return (
        <div className="card card-flush">
            <div className="card-header fw-bold">
                <div className="card-title">ADD QUOTE:</div>
                <div className="card-controls">
                </div>
            </div>
            <div className="card-body">
                <div className="form-container">
                    <form onSubmit={handleSubmit}>
                        <div className="form-row">
                            <label>Quote Title:</label>
                            <input
                                type="text"
                                value={quoteTitle}
                                onChange={(e) => setQuoteTitle(e.target.value)}
                                required
                            />
                            {
                                errors.filter(e => e.field === "QuoteTitle").map((e, index) => (
                                    <span key={index} style={{ color: 'red' }}>{e.message}</span>
                                ))
                            }
                        </div>
                        <div className="form-row">
                            <label>Client Name:</label>
                            <div><input
                                type="text"
                                value={clientName}
                                onChange={(e) => setClientName(e.target.value)}
                                readOnly
                                required
                            />
                                <button type="button" className="btn mlr-1" onClick={() => setShowPopup(true)}>Lookup Client</button>
                            </div>
                            {
                                errors.filter(e => e.field === "ClientId").map((e, index) => (
                                    <span key={index} style={{ color: 'red' }}>{e.message}</span>
                                ))
                            }
                        </div>
                        {showPopup && (
                            <ClientLookup
                                onSelect={(clientId, clientName) => {
                                    setClientId(clientId);
                                    setClientName(clientName);
                                    setShowPopup(false);
                                }}
                                onClose={() => setShowPopup(false)}
                            />
                        )}
                        <div className="form-row">
                            <label>Quote Date:</label>
                            <div><input
                                type="date"
                                value={quoteDate}
                                onChange={(e) => setQuoteDate(e.target.value)}
                                required
                            /></div>
                            {
                                errors.filter(e => e.field === "QuoteDate").map((e, index) => (
                                    <span key={index} style={{ color: 'red' }}>{e.message}</span>
                                ))
                            }
                        </div>
                        <button className="btn" type="submit">Save</button>
                        <button className="btn mlr-1" type="button" onClick={onCancel}>Cancel</button>
                    </form>
                </div>
            </div>
        </div>
    );
};

export default AddQuoteForm;
