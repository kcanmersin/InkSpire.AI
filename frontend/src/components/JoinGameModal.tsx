import { useState, useEffect } from "react";
import { Modal, Button, Form } from "react-bootstrap";
import axios from "axios";
import gameHubConnection from "../context/gameHubConnection";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

interface JoinGameModalProps {
  onClose: () => void;
}

const JoinGameModal = ({ onClose }: JoinGameModalProps) => {
  const [languages, setLanguages] = useState<string[]>([]);
  const [language, setLanguage] = useState("English");
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { userId } = useAuth();

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
        console.log("[JoinGameModal] Error fetching languages:", error);
      }
    };
    fetchLanguages();
  }, []);

  useEffect(() => {
    gameHubConnection.on("GameMatched", (roomId: string) => {
      console.log(`[SignalR] GameMatched event alındı - Room ID: ${roomId}`);
      onClose();
      navigate(`/game/${roomId}`);
    });

    return () => {
      gameHubConnection.off("GameMatched");
    };
  }, [navigate, onClose]);

  const handleJoinGame = async () => {
    if (!language) return;
    if (gameHubConnection.state !== "Connected") {
      console.error("SignalR bağlantısı kurulmadı.");
      return;
    }

    setLoading(true);
    await gameHubConnection.invoke("JoinGame", userId, language);
  };

  return (
    <Modal show onHide={onClose} centered>
      <Modal.Header closeButton>
        <Modal.Title>Join Game</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form.Group className="mb-3">
          <Form.Label>Select Language:</Form.Label>
          <Form.Select value={language} onChange={(e) => setLanguage(e.target.value)}>
            <option value="">Choose</option>
            {languages.map((lang) => (
              <option key={lang} value={lang}>{lang}</option>
            ))}
          </Form.Select>
        </Form.Group>
        <Button variant="primary" onClick={handleJoinGame} disabled={loading}>
          {loading ? "Waiting..." : "Join Game"}
        </Button>
      </Modal.Body>
    </Modal>
  );
};

export default JoinGameModal;
