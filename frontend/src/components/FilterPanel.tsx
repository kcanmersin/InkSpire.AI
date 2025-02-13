import React, { useState, useEffect } from 'react';
import axios from 'axios';

interface FilterPanelProps {
  selectedLanguages: string[];
  setSelectedLanguages: React.Dispatch<React.SetStateAction<string[]>>;
  selectedLevels: string[];
  setSelectedLevels: React.Dispatch<React.SetStateAction<string[]>>;
}

const FilterPanel: React.FC<FilterPanelProps> = ({
  selectedLanguages,
  setSelectedLanguages,
  selectedLevels,
  setSelectedLevels,
}) => {
  const [languages, setLanguages] = useState<string[]>([]);
  const [languageSearch, setLanguageSearch] = useState('');
  const levels = ['A1', 'A2', 'B1', 'B2', 'C1', 'C2'];

  useEffect(() => {
    axios.get('https://restcountries.com/v3.1/all')
      .then(response => {
        const langs = new Set<string>();
        response.data.forEach((country: any) => {
          if (country.languages) {
            Object.values(country.languages).forEach((lang: unknown) => {
              if (typeof lang === 'string') {
                langs.add(lang);
              }
            });
          }
        });
        setLanguages([...langs].sort());
      })
      .catch(err => console.error('Diller yüklenirken hata oluştu:', err));
  }, []);

  const handleLanguageChange = (selected: string) => {
    setSelectedLanguages(prev =>
      prev.includes(selected)
        ? prev.filter(lang => lang !== selected) 
        : [...prev, selected]
    );
  };

  const handleLevelChange = (selected: string) => {
    setSelectedLevels(prev =>
      prev.includes(selected)
        ? prev.filter(level => level !== selected) 
        : [...prev, selected] 
    );
  };

  return (
    <div className="col-md-3">
      <div className="bg-light p-3 shadow-sm rounded d-flex flex-column" style={{ minHeight: '80vh' }}>
        <h5 className="fw-bold mb-3">Filtrele</h5>

        <div className="mb-2">
          <input
            type="text"
            className="form-control"
            placeholder="Dil ara..."
            value={languageSearch}
            onChange={(e) => setLanguageSearch(e.target.value)}
          />
        </div>

        <div className="mb-3">
          <label htmlFor="languages" className="form-label">Dil Seçimi</label>
          <select
            id="languages"
            className="form-select"
            multiple
            size={8}
            value={selectedLanguages}
            onChange={(e) => handleLanguageChange(e.target.value)}
            style={{ maxHeight: '300px', overflowY: 'auto' }}
          >
            {languages
              .filter(lang => lang.toLowerCase().includes(languageSearch.toLowerCase()))
              .map((lang) => (
                <option key={lang} value={lang}>
                  {lang}
                </option>
              ))}
          </select>
        </div>

        <div className="mb-3">
          <label htmlFor="levels" className="form-label">Seviye Seçimi</label>
          <select
            id="levels"
            className="form-select"
            multiple
            size={6}
            value={selectedLevels}
            onChange={(e) => handleLevelChange(e.target.value)}
            style={{ maxHeight: '250px', overflowY: 'auto' }}
          >
            {levels.map((level) => (
              <option key={level} value={level}>
                {level}
              </option>
            ))}
          </select>
        </div>

        <button
          className="btn btn-danger mt-3"
          onClick={() => {
            setSelectedLanguages([]);
            setSelectedLevels([]);
            setLanguageSearch('');
          }}
        >
          Filtreleri Temizle
        </button>
      </div>
    </div>
  );
};

export default FilterPanel;
