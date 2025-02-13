import { useEffect, useState } from "react";
import axios from "axios";
import { Card, ListGroup, Spinner, Container } from "react-bootstrap";
import { useAuth } from "../context/AuthContext";

interface Word {
  id: string;
  wordText: string;
  translatedText: string;
  examples: string[];
  exampleDescriptions: string[];
}

const WordsPage = () => {
  const { userId } = useAuth();
  const [words, setWords] = useState<Word[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!userId) return;

    axios.get(`http://localhost:5256/api/Book/words?userId=${userId}`)
      .then((response) => setWords(response.data.value || []))
      .catch((error) => console.error("Words fetch error:", error))
      .finally(() => setLoading(false));
  }, [userId]);

  return (
    <Container className="mt-3 text-start">
      <h3 className="text-primary mb-3">My Words</h3>

      {loading ? (
        <div className="mt-3">
          <Spinner animation="border" role="status" />
        </div>
      ) : words.length === 0 ? (
        <p className="text-muted">You haven't saved any words yet.</p>
      ) : (
        <ListGroup variant="flush">
          {words.map((word) => (
            <ListGroup.Item key={word.id} className="p-2 border-0">
              <Card className="shadow-sm p-2">
                <Card.Body className="p-3">
                  <Card.Title className="text-primary fw-bold fs-5 mb-2">
                    {word.wordText} - {word.translatedText}
                  </Card.Title>
                  <ListGroup variant="flush" className="mt-1">
                    {word.examples.map((example, index) => (
                      <ListGroup.Item key={index} className="border-0 px-2 py-1">
                        <p className="fw-bold fs-6 mb-1">🔹 {example}</p>
                        <p className="text-muted fs-6">{word.exampleDescriptions[index]}</p>
                      </ListGroup.Item>
                    ))}
                  </ListGroup>
                </Card.Body>
              </Card>
            </ListGroup.Item>
          ))}
        </ListGroup>
      )}
    </Container>
  );
};

export default WordsPage;
