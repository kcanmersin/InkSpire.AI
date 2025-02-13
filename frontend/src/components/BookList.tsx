import React from 'react';
import { useQuery } from '@apollo/client';
import { useNavigate } from 'react-router-dom';
import { MAIN_PAGE_BOOK_LIST } from '../graphql/queries';
import BookCard from './BookCard';

interface BookListProps {
  selectedLanguages: string[];
  selectedLevels: string[];
}

const BookList: React.FC<BookListProps> = ({ selectedLanguages, selectedLevels }) => {
  const [offset, setOffset] = React.useState(0);
  const limit = 6;
  const navigate = useNavigate();

  const { loading, error, data } = useQuery(MAIN_PAGE_BOOK_LIST, {
    variables: { limit, offset, languages: selectedLanguages, levels: selectedLevels },
  });

  if (loading) return <p>Yükleniyor...</p>;
  if (error) return <p>Hata: {error.message}</p>;

  return (
    <div className="col-md-9">
      <div className="d-flex justify-content-between align-items-center mb-3">
        <button
          className="btn btn-outline-primary rounded-circle"
          onClick={() => setOffset((prev) => Math.max(prev - limit, 0))}
          disabled={offset === 0}
        >
          ❮
        </button>
        <button
          className="btn btn-outline-primary rounded-circle"
          onClick={() => setOffset((prev) => prev + limit)}
          disabled={data.books.length < limit}
        >
          ❯
        </button>
      </div>

      <div className="row">
        {data.books.map((book: any) => (
          <div
            key={book.id}
            className="col-md-6 col-lg-4 mb-3"
            onClick={() => navigate(`/book/${book.id}`)}
            style={{ cursor: 'pointer' }}
          >
            <div className="hover-card p-3 shadow-sm">
              <BookCard
                title={book.title}
                language={book.language}
                image={book.images?.length ? `data:image/jpeg;base64,${book.images[0].imageData}` : '/default.jpg'}
              />
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default BookList;
