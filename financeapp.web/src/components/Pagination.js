import React from 'react';

const Pagination = ({ page, setPage, totalCount, pageSize }) => {
    const totalPages = Math.ceil(totalCount / pageSize);

    return (
        <div className='ptb-4 right-flex'>
            <button className="btn" onClick={() => setPage(page - 1)} disabled={page === 1}>
                Previous
            </button>
            <span className='p-3'>{page} / {totalPages}</span>
            <button className="btn" onClick={() => setPage(page + 1)} disabled={page === totalPages}>
                Next
            </button>
        </div>
    );
};

export default Pagination;