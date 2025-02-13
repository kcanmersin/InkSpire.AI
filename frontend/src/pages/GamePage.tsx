import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import gameHubConnection from "../context/gameHubConnection";
import { useAuth } from "../context/AuthContext";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const GamePage = () => {
  const { roomId } = useParams();
  const { userId } = useAuth();

  const [topic, setTopic] = useState<string | null>(null);
  const [turnUserId, setTurnUserId] = useState<string | null>(null);
  const [input, setInput] = useState("");
  const [gameState, setGameState] = useState<string>("Waiting for topic...");
  const [messages, setMessages] = useState<{ user: string; text: string; correct: boolean; duplicate?: boolean }[]>([]);
  const [usedAnswers, setUsedAnswers] = useState<Set<string>>(new Set());

  

  useEffect(() => {
    console.log("[GamePage] Component Mounted - Room ID:", roomId);

    gameHubConnection.on("GameTopic", (newTopic: string) => {
      console.log("[GamePage] Topic received:", newTopic);
      setTopic(newTopic);
      setGameState(`Topic: ${newTopic}`);
    });

    gameHubConnection.on("StartGame", (startingUserId: string) => {
      console.log("[GamePage] StartGame event received! First player:", startingUserId);
      setTurnUserId(startingUserId);
    });

    gameHubConnection.on("CorrectAnswer", (user: string, answer: string) => {
      console.log(`[GamePage] Correct answer received: ${answer} by ${user}`);
      setMessages((prev) => [...prev, { user, text: answer, correct: true }]);
      setUsedAnswers((prev) => new Set(prev).add(answer.toLowerCase())); 
      setTurnUserId(user === userId ? null : user); 
    });

    gameHubConnection.on("WrongAnswer", (user: string, answer: string) => {
      console.log(`[GamePage] Wrong answer received: ${answer} by ${user}`);
      setMessages((prev) => [...prev, { user, text: answer, correct: false }]);
      setTurnUserId(user);
    });

    gameHubConnection.on("DuplicateAnswer", (user: string, answer: string) => {
      console.log(`[GamePage] Duplicate answer attempted: ${answer}`);
      toast.error(`"${answer}" has already been used! Try a different word.`, {
        position: "top-right",
        autoClose: 3000,
      });

      setMessages((prev) => [...prev, { user, text: answer, correct: false, duplicate: true }]);
    });

    return () => {
      console.log("[GamePage] Cleaning up event listeners...");
      gameHubConnection.off("GameTopic");
      gameHubConnection.off("StartGame");
      gameHubConnection.off("CorrectAnswer");
      gameHubConnection.off("WrongAnswer");
      gameHubConnection.off("DuplicateAnswer");
    };
  }, [roomId, userId]);

  const requestTopic = () => {
    console.log("[GamePage] Requesting topic...");
    gameHubConnection.invoke("RequestTopic", roomId)
      .then(() => console.log("[GamePage] RequestTopic invoked successfully"))
      .catch((err) => console.error("[GamePage] RequestTopic invocation failed:", err));
  };

  const submitAnswer = () => {
    if (input.trim() !== "") {
      const answer = input.trim().toLowerCase();

      if (usedAnswers.has(answer)) {
        toast.error(`"${answer}" has already been used! Try a different word.`, {
          position: "top-right",
          autoClose: 3000,
        });
        return;
      }

      console.log("[GamePage] Submitting answer:", input);
      gameHubConnection.invoke("SubmitAnswer", roomId, userId, input)
        .then(() => {
          console.log("[GamePage] Answer submitted successfully");
          setInput("");
        })
        .catch(err => console.error("[GamePage] SubmitAnswer invocation failed:", err));
    }
  };

  return (
    <div style={{ textAlign: "center", padding: "20px" }}>
      <h1>Game Room: {roomId}</h1>
      <h2>{gameState}</h2>

      {!topic && <button onClick={requestTopic}>Get Topic</button>}

      <div style={{ maxHeight: "300px", overflowY: "auto", border: "1px solid gray", padding: "10px", marginBottom: "10px" }}>
        {messages.map((msg, index) => (
          <div key={index} style={{ 
            backgroundColor: msg.duplicate ? "darkred" : msg.correct ? "lightgreen" : "lightcoral", 
            color: msg.duplicate ? "white" : "black",
            padding: "5px", 
            margin: "5px", 
            borderRadius: "5px",
            textAlign: msg.user === userId ? "right" : "left"
          }}>
            <b>{msg.user === userId ? "You" : "Opponent"}:</b> {msg.text}
          </div>
        ))}
      </div>

      <input 
        type="text" 
        value={input} 
        onChange={(e) => setInput(e.target.value)} 
        disabled={turnUserId !== userId} 
      />
      <button onClick={submitAnswer} disabled={turnUserId !== userId}>Submit</button>
    </div>
  );
};

export default GamePage;
