import React, { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import AddQuoteForm from './AddQuoteForm';
import ViewQuote from './ViewQuote';
import QuotesList from './QuotesList';
import { Actions } from '../utilities/constants.js'

function QuotesWrapper() {

    const [currentAction, setCurrentAction] = useState(Actions.LIST);
    const [quoteId, setQuoteId] = useState(0);
    const location = useLocation();

    useEffect(() => {
        setCurrentAction(Actions.LIST);
      }, [location]);

    const handleSaveQuote = (newQuote) => {
    };

    const handleCancelQuote = () => {
        setCurrentAction(Actions.LIST)
    };

    const handleAddQuote = () => {
        setCurrentAction(Actions.ADD)
    }

    const handleViewQuote = (quoteId) => {
        setCurrentAction(Actions.VIEW)
        setQuoteId(quoteId);
    }

    return (
    <div>
        {currentAction === Actions.LIST ? (
            <QuotesList onAdd={handleAddQuote} onSelect={handleViewQuote} />
        ) : currentAction === Actions.ADD ? (
            <AddQuoteForm onSave={handleSaveQuote} onCancel={handleCancelQuote} />
        ) : currentAction === Actions.VIEW ? (
            <ViewQuote onCancel={handleCancelQuote} quoteId={quoteId} />
        ) : null}
    </div>);
}

export default QuotesWrapper;