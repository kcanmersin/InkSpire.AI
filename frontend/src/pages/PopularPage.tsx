import{ useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

const PopularPage = () => {
  const [books, setBooks] = useState<any[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchPopularBooks = async () => {
      try {
        const response = await axios.get('http://localhost:5256/api/Book/popular-books?limit=9');
        setBooks(response.data);
      } catch (error) {
        console.error('Error fetching popular books:', error);
      }
    };

    fetchPopularBooks();
  }, []);

  return (
    <div className="container mt-4">
      <h2>Popular Books</h2>
      <div className="row">
        {books.map((book) => (
          <div
            key={book.id}
            className="col-md-4 mb-3"
            onClick={() => navigate(`/book/${book.id}`)}
            style={{ cursor: 'pointer' }}
          >
            <div className="card">
              <img
                src={book.bookImage ? `data:image/jpeg;base64,${book.bookImage}` : '/default.jpg'}
                alt={book.title}
                className="card-img-top"
                style={{ objectFit: 'cover', height: '200px' }}
              />
              <div className="card-body">
                <h5 className="card-title">{book.title}</h5>
                <p className="card-text">
                  Level: <strong>{book.level}</strong> | Language: <strong>{book.language}</strong>
                </p>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default PopularPage;
