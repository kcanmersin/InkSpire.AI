import React, { useEffect, useState } from "react";
import { Modal, Button, Spinner, ListGroup, Card, Alert } from "react-bootstrap";
import { translateText, generateExamplesWithExplanations } from "../api/chatgroqapi";
import { useAuth } from "../context/AuthContext";
import axios from "axios";

interface TranslationPopupProps {
  selectedText: string | null;
  onClose: () => void;
  bookLanguage: string;
}

const TranslationPopup: React.FC<TranslationPopupProps> = ({ selectedText, onClose, bookLanguage }) => {
  const [translatedText, setTranslatedText] = useState<string | null>(null);
  const [examplesWithExplanations, setExamplesWithExplanations] = useState<
    { sentence: string; explanation: string }[]
  >([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [isSaving, setIsSaving] = useState<boolean>(false);
  const [saveSuccess, setSaveSuccess] = useState<boolean | null>(null);
  const [nativeLanguage, setNativeLanguage] = useState<string | null>(null);
  const { userId } = useAuth();

  useEffect(() => {
    if (!userId) return;
    axios.get(`http://localhost:5256/api/User/${userId}`)
      .then((res) => setNativeLanguage(res.data.nativeLanguage))
      .catch(() => setNativeLanguage(null));
  }, [userId]);

  useEffect(() => {
    if (!selectedText || !nativeLanguage) return;

    const fetchData = async () => {
      setIsLoading(true);
      const translation = await translateText(selectedText, nativeLanguage);
      const examples = await generateExamplesWithExplanations(selectedText, bookLanguage);
      setTranslatedText(translation);
      setExamplesWithExplanations(examples);
      setIsLoading(false);
    };

    fetchData();
  }, [selectedText, nativeLanguage, bookLanguage]);

  const handleSaveWord = async () => {
    if (!selectedText || !translatedText || examplesWithExplanations.length === 0) return;

    setIsSaving(true);
    setSaveSuccess(null);

    const payload = {
      userId,
      wordText: selectedText,
      translatedText,
      examples: examplesWithExplanations.map((ex) => ex.sentence),
      exampleDescriptions: examplesWithExplanations.map((ex) => ex.explanation)
    };

    try {
      const res = await fetch("http://localhost:5256/api/Book/word", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload),
      });

      if (!res.ok) throw new Error("Failed to save word.");

      setSaveSuccess(true);
    } catch {
      setSaveSuccess(false);
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <Modal show={!!selectedText} onHide={onClose} centered>
      <Modal.Header closeButton className="bg-primary text-white">
        <Modal.Title style={{ userSelect: "none" }}>🌍 Translation</Modal.Title>
      </Modal.Header>
      <Modal.Body className="p-4">
        {!nativeLanguage ? (
          <div className="text-center my-3" style={{ userSelect: "none" }}>
            <Spinner animation="border" size="sm" />
            <p>Loading user language...</p>
          </div>
        ) : (
          <>
            <Card className="mb-3 border-0 shadow-sm">
              <Card.Body className="bg-light p-3 rounded">
                <h6 className="text-secondary" style={{ userSelect: "none" }}>Selected Text:</h6>
                <p className="text-dark fw-bold" style={{ userSelect: "none" }}>{selectedText}</p>
              </Card.Body>
            </Card>

            <Card className="mb-3 border-0 shadow-sm">
              <Card.Body className="p-3 rounded">
                <h6 className="text-secondary" style={{ userSelect: "none" }}>Translated Text ({nativeLanguage}):</h6>
                {isLoading ? <Spinner animation="border" size="sm" /> : <p className="text-success fw-bold" style={{ userSelect: "none" }}>{translatedText}</p>}
              </Card.Body>
            </Card>

            <h6 className="text-secondary" style={{ userSelect: "none" }}>📖 Example Sentences with Explanations ({bookLanguage}):</h6>
            {isLoading ? (
              <Spinner animation="border" size="sm" className="my-3" />
            ) : examplesWithExplanations.length > 0 ? (
              <ListGroup variant="flush" className="mt-3">
                {examplesWithExplanations.map((example, index) => (
                  <ListGroup.Item key={index} className="border-0" style={{ userSelect: "none" }}>
                    <div className="p-3 bg-light rounded">
                      <p className="fw-bold mb-1">{index + 1}. {example.sentence}</p>
                      <p className="text-muted mb-0">{example.explanation}</p>
                    </div>
                  </ListGroup.Item>
                ))}
              </ListGroup>
            ) : (
              <p className="text-muted" style={{ userSelect: "none" }}>No examples available.</p>
            )}
          </>
        )}

        {saveSuccess !== null && (
          <Alert variant={saveSuccess ? "success" : "danger"} className="mt-3" style={{ userSelect: "none" }}>
            {saveSuccess ? "✅ Success" : "❌ Failed to save word."}
          </Alert>
        )}
      </Modal.Body>

      <Modal.Footer className="border-0">
        <Button variant="success" onClick={handleSaveWord} disabled={isSaving} style={{ userSelect: "none" }}>
          {isSaving ? "Saving..." : "💾 Save Word"}
        </Button>
        <Button variant="outline-secondary" onClick={onClose} style={{ userSelect: "none" }}>
          Close
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default TranslationPopup;
