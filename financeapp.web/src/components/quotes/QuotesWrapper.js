import React, { useState } from 'react';
import AddQuoteForm from './AddQuoteForm';
import QuotesList from './QuotesList';

function QuotesWrapper() {

    const [isAddingQuote, setIsAddingQuote] = useState(false);

    const handleAddQuoteClick = () => {
        setIsAddingQuote(true);
    };

    const handleSaveQuote = (newQuote) => {
        setIsAddingQuote(false);
    };

    const handleCancelQuote = () => {
        setIsAddingQuote(false);
    };

    return (<div>
        {
            isAddingQuote ? (
                <AddQuoteForm onSave={handleSaveQuote} onCancel={handleCancelQuote} />
            ) : (
                <QuotesList onAdd={handleAddQuoteClick} />
            )
        }
    </div>);
}

export default QuotesWrapper;