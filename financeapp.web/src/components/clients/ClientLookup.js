import React, { useState } from 'react';
import axios from 'axios';

const ClientLookup = ({ onSelect, onClose }) => {
    const [searchTerm, setSearchTerm] = useState('');
    const [searchResults, setSearchResults] = useState([]);
    const [searchSuccessful, setSearchSuccessful] = useState(false);

    const handleSearch = async (e) => {

        e.preventDefault();

        try {
            const response = await axios.get(`${process.env.REACT_APP_API_URL}/clients/search?search=${searchTerm}`);
            setSearchResults(response.data);
            setSearchSuccessful(true);
        } catch (error) {
            console.error('Error searching clients:', error);
            setSearchSuccessful(false);
        }
    };

    return (
        <div className="popup-backdrop">
            <div className="popup-inner">
            <div className="fb m-1">SEARCH FOR CLIENT:</div>
                <div className="m-1">
                    <input
                        type="text"
                        placeholder="Enter search term..."
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                    />

                    <button className="btn mlr-1" onClick={handleSearch}>Search</button>
                    <button className="btn" onClick={onClose}>Close</button>
                </div>
                <div className="m-1">
                    {
                        searchResults.length > 0 ? (
                            <table className="responsive-table">
                                <tbody>
                                    {searchResults.map((client) => (
                                        <tr key={client.id}>
                                            <td data-label="Name:"><button className="button-as-link" onClick={() => {onSelect(client.id, client.firstName + ' ' + client.lastName)}}>{client.firstName + ' ' + client.lastName}</button></td>
                                            <td data-label="Address:">{client.addressLine1 + ' ' + client.postcode}</td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>) : (
                            searchSuccessful ? (
                                <div>No results found for this search term</div>
                            ) : (<div></div>)
                        )}
                </div>
            </div>
        </div>
    );
};

export default ClientLookup;
