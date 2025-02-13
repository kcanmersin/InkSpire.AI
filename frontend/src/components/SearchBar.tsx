import React, { useState } from 'react';
import axios from 'axios';

interface SearchBarProps {
  onSearchResults: (results: any[]) => void;
}

const SearchBar: React.FC<SearchBarProps> = ({ onSearchResults }) => {
  const [query, setQuery] = useState('');

  const handleSearch = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setQuery(value);

    if (value.trim()) {
      try {
        // API'den veri çekiyoruz
        const response = await axios.get('http://localhost:5256/api/Search/search', {
          params: { title: value }, // `title` parametresini API'ye gönder
        });

        // Gelen veriyi üst bileşene gönder
        onSearchResults(response.data);
        //log response
        console.log(response.data);
      } catch (error: any) {
        console.error('API çağrısı sırasında hata oluştu:', error.message);
        onSearchResults([]); // Hata durumunda sonuçları temizle
      }
    } else {
      onSearchResults([]); // Arama alanı boşsa sonuçları sıfırla
    }
  };

  return (
    <div className="mb-3">
      <input
        type="text"
        className="form-control"
        placeholder="Search for books..."
        value={query}
        onChange={handleSearch}
      />
    </div>
  );
};

export default SearchBar;
