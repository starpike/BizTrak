import React, { useState } from 'react';
import axios from 'axios';
import '../../css/forms.css';

const AddQuoteForm = ({ onSave, onCancel }) => {
    const [quoteTitle, setQuoteTitle] = useState('');
    const [clientId, setClientId] = useState('');
    const [quoteDate, setQuoteDate] = useState('');

    const handleSubmit = async (event) => {
        event.preventDefault();
        const newQuote = { quoteTitle, quoteDate, clientId };
        try {
            const response = await axios.post(`${process.env.REACT_APP_API_URL}/quotes`, newQuote);
            onSave(response.data);
        } catch (error) {
            console.error('Error creating quote:', error);
        }
    };

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
                        </div>
                        <div className="form-row">
                            <label>Client Name:</label>
                            <input
                                type="text"
                                value={clientId}
                                onChange={(e) => setClientId(e.target.value)}
                                required
                            />
                        </div>
                        <div className="form-row">
                            <label>Quote Date:</label>
                            <input
                                type="date"
                                value={quoteDate}
                                onChange={(e) => setQuoteDate(e.target.value)}
                                required
                            />
                        </div>
                        <button className="btn" type="submit">Save</button>
                        <button  className="btn mlr-1" type="button" onClick={onCancel}>Cancel</button>
                    </form>
                </div>
            </div>
        </div>
    );
};

export default AddQuoteForm;
