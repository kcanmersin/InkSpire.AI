import { useEffect, useState } from "react";
import axios from "axios";
import { Card, Spinner, Button, Modal, Form } from "react-bootstrap";
import { useAuth } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";

interface Book {
  id: string;
  title: string;
  language: string;
  level: string;
  imageUrl: string | null;
}

const Profile = () => {
  const { userId } = useAuth();
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(true);
  const [showPreferences, setShowPreferences] = useState(false);
  const [userData, setUserData] = useState({
    name: "",
    surname: "",
    nativeLanguage: "",
    targetLanguage: "",
  });
  const [languages, setLanguages] = useState<string[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    if (!userId) return;

    axios.get(`http://localhost:5256/api/Book/profile/${userId}`)
      .then((response) => setBooks(response.data))
      .catch((error) => console.error("Kitaplar yüklenirken hata oluştu:", error))
      .finally(() => setLoading(false));

    axios.get(`http://localhost:5256/api/User/${userId}`)
      .then((response) => setUserData(response.data))
      .catch((error) => console.error("Kullanıcı bilgileri yüklenirken hata oluştu:", error));

    axios.get("https://restcountries.com/v3.1/all")
      .then((response) => {
        const langs = new Set<string>();
        response.data.forEach((country: any) => {
          if (country.languages) {
            Object.values(country.languages).forEach((lang: unknown) => {
              if (typeof lang === "string") {
                langs.add(lang);
              }
            });
          }
        });
        setLanguages([...langs].sort());
      })
      .catch((err) => console.error("Diller yüklenirken hata oluştu:", err));
  }, [userId]);

  const handleUpdate = () => {
    axios.put("http://localhost:5256/api/User/update", { userId, ...userData })
      .then(() => {
        alert("Bilgileriniz başarıyla güncellendi.");
        setShowPreferences(false);
      })
      .catch((error) => alert("Güncelleme sırasında hata oluştu: " + error.message));
  };

  return (
    <div className="container mt-4">
      <h1 className="text-primary text-center">Profilim</h1>
      
      <div className="d-flex justify-content-center mb-4">
        <Button variant="outline-primary" onClick={() => setShowPreferences(true)}>
          Preferences
        </Button>
        <Button
          variant="outline-success"
          className="ms-2"
          onClick={() => navigate("/words")}
        >
          📖 My Words
        </Button>
      </div>

      {loading ? (
        <div className="text-center mt-4">
          <Spinner animation="border" role="status" />
        </div>
      ) : books.length === 0 ? (
        <p className="text-center text-muted">Henüz eklenmiş bir kitabınız yok.</p>
      ) : (
        <div className="row">
          {books.map((book) => (
            <div key={book.id} className="col-md-4 mb-4">
              <Card className="shadow-sm h-100" onClick={() => navigate(`/book/${book.id}`)} style={{ cursor: "pointer" }}>
                <Card.Img
                  variant="top"
                  src={book.imageUrl ? `data:image/jpeg;base64,${book.imageUrl}` : "/default-book.jpg"}
                  alt="Book Cover"
                  className="img-fluid"
                  style={{ height: "200px", objectFit: "cover" }}
                />
                <Card.Body>
                  <Card.Title className="fw-bold">{book.title}</Card.Title>
                  <Card.Text className="text-muted">
                    <strong>Dil:</strong> {book.language} <br />
                    <strong>Seviye:</strong> {book.level}
                  </Card.Text>
                </Card.Body>
              </Card>
            </div>
          ))}
        </div>
      )}

      <Modal show={showPreferences} onHide={() => setShowPreferences(false)} centered>
        <Modal.Header closeButton>
          <Modal.Title>Update Preferences</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form>
            <Form.Group>
              <Form.Label>Name</Form.Label>
              <Form.Control
                type="text"
                value={userData.name}
                onChange={(e) => setUserData({ ...userData, name: e.target.value })}
              />
            </Form.Group>

            <Form.Group>
              <Form.Label>Surname</Form.Label>
              <Form.Control
                type="text"
                value={userData.surname}
                onChange={(e) => setUserData({ ...userData, surname: e.target.value })}
              />
            </Form.Group>

            <Form.Group>
              <Form.Label>Native Language</Form.Label>
              <Form.Select
                value={userData.nativeLanguage}
                onChange={(e) => setUserData({ ...userData, nativeLanguage: e.target.value })}
              >
                <option value="">Select a language</option>
                {languages.map((lang) => (
                  <option key={lang} value={lang}>
                    {lang}
                  </option>
                ))}
              </Form.Select>
            </Form.Group>

            <Form.Group>
              <Form.Label>Target Language</Form.Label>
              <Form.Select
                value={userData.targetLanguage}
                onChange={(e) => setUserData({ ...userData, targetLanguage: e.target.value })}
              >
                <option value="">Select a language</option>
                {languages.map((lang) => (
                  <option key={lang} value={lang}>
                    {lang}
                  </option>
                ))}
              </Form.Select>
            </Form.Group>
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowPreferences(false)}>
            Cancel
          </Button>
          <Button variant="primary" onClick={handleUpdate}>
            Save Changes
          </Button>
        </Modal.Footer>
      </Modal>


    </div>
  );
};

export default Profile;
