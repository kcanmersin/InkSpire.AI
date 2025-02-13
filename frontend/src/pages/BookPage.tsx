import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import BookList from '../components/BookList';
import FilterPanel from '../components/FilterPanel';
import SearchBar from '../components/SearchBar';

const BookPage = () => {
  const [selectedLanguages, setSelectedLanguages] = useState<string[]>([]);
  const [selectedLevels, setSelectedLevels] = useState<string[]>([]);
  const [searchResults, setSearchResults] = useState<any[]>([]);
  const navigate = useNavigate();

  return (
    <div className="container mt-4">
      <div className="d-flex justify-content-between align-items-center mb-3">
        {/* Populars Butonu */}
        <button className="btn btn-primary" onClick={() => navigate('/popular-books')}>
          Populars
        </button>
      </div>

      {/* Search bar */}
      <SearchBar onSearchResults={setSearchResults} />
      {searchResults.length > 0 && (
        <div className="dropdown-menu show w-100 mt-2">
          {searchResults.map((book: any) => (
            <div
              key={book.id}
              className="dropdown-item d-flex align-items-center"
              onClick={() => navigate(`/book/${book.id}`)}
              style={{ cursor: 'pointer' }}
            >
              <div className="me-3">
                <img
                  src={book.images?.length > 0 ? book.images[0] : '/default.jpg'}
                  alt={book.title}
                  className="rounded"
                  style={{ width: '50px', height: '50px', objectFit: 'cover' }}
                />
              </div>
              <div>
                <p className="mb-1 fw-bold">{book.title}</p>
                <p className="mb-0 text-muted small">{book.language || 'No language info'}</p>
              </div>
            </div>
          ))}
        </div>
      )}
      
      <div className="row">
        <FilterPanel
          selectedLanguages={selectedLanguages}
          setSelectedLanguages={setSelectedLanguages}
          selectedLevels={selectedLevels}
          setSelectedLevels={setSelectedLevels}
        />
        <BookList selectedLanguages={selectedLanguages} selectedLevels={selectedLevels} />
      </div>
    </div>
  );
};

export default BookPage;
