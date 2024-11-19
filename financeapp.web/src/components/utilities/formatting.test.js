import '@testing-library/jest-dom';
import { formatDate } from '../utilities/formatting';


describe('QuotesList Component', () => {
    it ('formats a date object correctly using dd/MM/yyyy', () => {
        const testDates = [
            "2024-07-08",
            "2024-07-08T12:00:00Z",
            "2024-07-08T12:00:00.000Z",
            "07/08/2024",
            "7/8/2024",
            "July 8, 2024",
            "Jul 8, 2024",
            "07/08/2024 14:30",
            "07/08/2024 2:30 PM",
            "Mon, 08 Jul 2024 12:00:00 GMT",
            "8-July-2024",
            "2024.07.08",
            "2024/07/08"
          ];

        testDates.forEach(dateString => {
            var formattedDate = formatDate(dateString);
            expect(formattedDate).toBe('08/07/2024');
        });
    });
});