import { useState, useEffect } from "react";
import { Modal, Button, Form, Spinner } from "react-bootstrap";
import { useAuth } from "../context/AuthContext";
import axios from "axios";
import hubConnection from "../context/signalr";

const CreateBookModal = ({ onClose }: { onClose: () => void }) => {
  const { userId } = useAuth();
  const [title, setTitle] = useState("");
  const [language, setLanguage] = useState("");
  const [level, setLevel] = useState("A1");
  const [languages, setLanguages] = useState<string[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchLanguages = async () => {
      try {
        const response = await axios.get("https://restcountries.com/v3.1/all");
        const languageSet = new Set<string>();
        response.data.forEach((country: any) => {
          if (country.languages) {
            Object.values<string>(country.languages).forEach((lang) => languageSet.add(lang));
          }
        });
        setLanguages(Array.from(languageSet).sort());
      } catch (error) {
        console.log("[CreateBookModal] Error fetching languages:", error);
      }
    };
    fetchLanguages();

    const handleBookCreated = (bookTitle: string) => {
      console.log(`[CreateBookModal] SignalR bookcreated event: ${bookTitle}`);
    };

    hubConnection.on("bookcreated", handleBookCreated);

    return () => {
      console.log("[CreateBookModal] Cleanup: removing bookcreated listener");
      hubConnection.off("bookcreated", handleBookCreated);
    };
  }, []);

  const handleSubmit = async () => {
    setLoading(true);
    console.log("[CreateBookModal] Submit clicked");
    try {
      await axios.post("http://localhost:5256/api/Book/create", {
        authorId: userId,
        title,
        language,
        level,
      });
      console.log("[CreateBookModal] Book create request sent");
      onClose();
      setTitle("");
      setLanguage("");
      setLevel("A1");
    } catch (error) {
      console.log("[CreateBookModal] Error creating book:", error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal show onHide={onClose} centered>
      <Modal.Header closeButton>
        <Modal.Title>Create a New Book</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form>
          <Form.Group className="mb-3">
            <Form.Label>Title</Form.Label>
            <Form.Control
              type="text"
              placeholder="Enter book title"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Language</Form.Label>
            <Form.Select value={language} onChange={(e) => setLanguage(e.target.value)}>
              <option value="" disabled>Select a language</option>
              {languages.map((lang) => (
                <option key={lang} value={lang}>{lang}</option>
              ))}
            </Form.Select>
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Level</Form.Label>
            <Form.Select value={level} onChange={(e) => setLevel(e.target.value)}>
              {["A1", "A2", "B1", "B2", "C1", "C2"].map((lvl) => (
                <option key={lvl} value={lvl}>{lvl}</option>
              ))}
            </Form.Select>
          </Form.Group>
        </Form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onClose}>
          Cancel
        </Button>
        <Button variant="primary" onClick={handleSubmit} disabled={loading}>
          {loading ? <Spinner animation="border" size="sm" /> : "Create"}
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default CreateBookModal;
