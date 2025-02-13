import React from 'react';

interface BookCardProps {
  title: string;
  language: string;
  image?: string | null;
}

const BookCard: React.FC<BookCardProps> = ({ title, language, image }) => {
  const defaultImage = '/default.jpg';

  return (
    <div className="card shadow-sm border-0 h-100">
      <img
        src={image || defaultImage}
        className="card-img-top rounded-top"
        alt={title}
        style={{ height: '200px', objectFit: 'cover' }}
      />
      <div className="card-body d-flex flex-column">
        <h5 className="card-title text-truncate">{title}</h5>
        <p className="card-text text-muted">Dil: {language}</p>
      </div>
    </div>
  );
};

export default BookCard;
