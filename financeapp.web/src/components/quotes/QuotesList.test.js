import React from 'react';
import { render, screen, fireEvent, waitFor, within } from '@testing-library/react';
import '@testing-library/jest-dom';
import axios from 'axios';
import { formatDate } from '../utilities/formatting';
import QuotesList from './QuotesList';
import Pagination from '../Pagination';

jest.mock('axios');

jest.mock('../Pagination', () => jest.fn());

const paginationMock = ({ page, setPage, totalCount, pageSize }) => (
    <div>
        <button onClick={() => setPage(page - 1)} disabled={page === 1}>Previous</button>
        <button onClick={() => setPage(page + 1)}>Next</button>
    </div>
)

describe('QuotesList Component', () => {

    const originalEnv = process.env;
    const consoleError = console.error;

    beforeEach(() => {
        // Create a deep copy of process.env to restore later
        process.env = { ...originalEnv, REACT_APP_API_URL: 'http://mocked.api.url' };
        console.error = jest.fn();
    });

    afterEach(() => {
        process.env = originalEnv;
        console.error = consoleError;
    });

    const mockQuotes = [
        {
            id: 1,
            quoteRef: 'Q123',
            quoteTitle: 'Sample Quote 1',
            client: { firstName: 'John', lastName: 'Doe', addressLine1: '123 Street' },
            quoteDate: '2022-01-01',
            statusName: 'New'
        },
        {
            id: 2,
            quoteRef: 'Q124',
            quoteTitle: 'Sample Quote 2',
            client: { firstName: 'Jane', lastName: 'Smith', addressLine1: '456 Avenue' },
            quoteDate: '2022-02-02',
            statusName: 'New'
        }
    ];

    it('renders without crashing', async () => {
        axios.get.mockResolvedValue({ data: { quotes: mockQuotes, total: 2, page: 1, pageSize: 1 } });

        render(<QuotesList onAdd={jest.fn()} onSelect={jest.fn()} />);

        await screen.findByText('Sample Quote 1');

        expect(screen.getByPlaceholderText('search quotes...')).toBeInTheDocument();
        expect(screen.getByText('Add Quote')).toBeInTheDocument();
        expect(screen.getByText('Sample Quote 2')).toBeInTheDocument();
    });

    it('displays error message on API error', async () => {
        axios.get.mockRejectedValue(new Error('Failed to load quotes'));

        render(<QuotesList onAdd={jest.fn()} onSelect={jest.fn()} />);

        await screen.findByText('Error: Failed to load quotes');
        expect(console.error).toHaveBeenCalledTimes(1);
    });

    it('calls onAdd when Add Quote button is clicked', () => {
        const onAddMock = jest.fn();

        render(<QuotesList onAdd={onAddMock} onSelect={jest.fn()} />);

        fireEvent.click(screen.getByText('Add Quote'));
        expect(onAddMock).toHaveBeenCalledTimes(1);
    });

    it('filters quotes based on search input', async () => {
        axios.get.mockResolvedValue({ data: { data: mockQuotes, totalCount: 2, page: 1, pageSize: 1 } });

        render(<QuotesList onAdd={jest.fn()} onSelect={jest.fn()} />);

        fireEvent.change(screen.getByPlaceholderText('search quotes...'), { target: { value: 'Sample Quote 1' } });

        expect(axios.get).toHaveBeenCalledWith(`${process.env.REACT_APP_API_URL}/quotes/search`, {
            params: { page: 1, pageSize: 10, search: 'Sample Quote 1' }
        });
    });

    it('paginates quotes', async () => {
        axios.get.mockResolvedValue({ data: { quotes: mockQuotes, total: 2, page: 1, pageSize: 1 } });

        Pagination.mockImplementation(paginationMock);

        render(<QuotesList onAdd={jest.fn()} onSelect={jest.fn()} />);

        await screen.findByText('Sample Quote 1');

        fireEvent.click(screen.getByText('Next'));

        await waitFor(() => {
            expect(Pagination).toHaveBeenCalledWith(
                expect.objectContaining({ page: 2 }),
                expect.anything()
            );
        });
    });

    it('display a status of NEW for each quote', async () => {
        axios.get.mockResolvedValue({ data: { quotes: mockQuotes, total: 2, page: 1, pageSize: 1 } });

        render(<QuotesList onAdd={jest.fn()} onSelect={jest.fn()} />);

        await waitFor(() => {

            const rows = screen.getAllByRole('row');
            expect(rows).toHaveLength(mockQuotes.length + 1); // +1 for the header row        

            mockQuotes.forEach((quote, index) => {

                const row = rows[index + 1]; // Skip the header row
                const utils = within(row);

                expect(utils.getByText(quote.quoteRef)).toBeInTheDocument();
                expect(utils.getByText(quote.quoteTitle)).toBeInTheDocument();
                expect(utils.getByText(quote.client.firstName + ' ' + quote.client.lastName)).toBeInTheDocument();
                expect(utils.getByText(formatDate(quote.quoteDate))).toBeInTheDocument();
                expect(utils.getByText(quote.statusName)).toBeInTheDocument();
            });
        });
    });
});
