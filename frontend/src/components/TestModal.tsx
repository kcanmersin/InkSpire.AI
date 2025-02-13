import { useState, useEffect } from "react";
import axios from "axios";
import { Modal, Button, Form, Spinner } from "react-bootstrap";

interface Question {
  questionId: string;
  questionText: string;
  score: number;
  answer: string;
  choices: string[];
  questionType: string;
  feedback?: string;
}

interface TestData {
  testId: string;
  bookId: string;
  userId?: string;
  totalScore: number;
  generalFeedback: string;
  questions: Question[];
}

const TestModal = ({ bookId, userId, onClose }: { bookId: string; userId: string; onClose: () => void }) => {
  const [testData, setTestData] = useState<TestData | null>(null);
  const [answers, setAnswers] = useState<{ questionText: string; answer: string }[]>([]);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [showFeedback, setShowFeedback] = useState(false);

  useEffect(() => {
    const fetchTest = async () => {
      try {
        const response = await axios.get(`http://localhost:5256/api/Book/test?BookId=${bookId}&UserId=${userId || ""}`);
        const fetchedTest = response.data.value;
        setTestData(fetchedTest);
        setAnswers(
          fetchedTest.questions.map((q: Question) => ({
            questionText: q.questionText,
            answer: q.answer || "",
          }))
        );
        setShowFeedback(fetchedTest.questions.some((q: Question) => q.feedback));
      } catch (error) {
        console.error("Error fetching test:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchTest();
  }, [bookId, userId]);

  useEffect(() => {
    if (testData?.questions.some((q) => q.feedback)) {
      setShowFeedback(true);
    }
  }, [testData]);

  const handleAnswerChange = (questionText: string, answer: string) => {
    setAnswers((prev) => {
      const updated = prev.filter((q) => q.questionText !== questionText);
      return [...updated, { questionText, answer }];
    });
  };

  const handleSubmit = async () => {
    if (!testData) return;
    setSubmitting(true);
    setShowFeedback(false);

    try {
      const response = await axios.post("http://localhost:5256/api/Book/solve-test", {
        bookId: testData?.bookId || bookId,
        userId,
        answers,
      });

      const updatedTest = response.data.value;
      setTestData(updatedTest);
      setAnswers(
        updatedTest.questions.map((q: Question) => ({
          questionText: q.questionText,
          answer: q.answer || "",
        }))
      );
    } catch (error) {
      console.error("Error submitting test:", error);
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <Modal show onHide={onClose} size="lg" centered>
      <Modal.Header closeButton>
        <Modal.Title>Test</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        {loading ? (
          <div className="text-center">
            <Spinner animation="border" />
            <p>Loading test...</p>
          </div>
        ) : (
          <>
            <div className="alert alert-primary">
              <h5>Overall Score: {testData?.totalScore}/100</h5>
            </div>

            {testData?.questions.map((q) => (
              <div key={q.questionText} className="mb-4">
                <p className="fw-bold">{q.questionText}</p>
                <p className="text-muted">Score: {q.score}</p>
                {q.choices.length > 0 ? (
                  <Form.Select
                    value={answers.find((a) => a.questionText === q.questionText)?.answer || ""}
                    onChange={(e) => handleAnswerChange(q.questionText, e.target.value)}
                  >
                    <option value="" disabled>Select an answer</option>
                    {q.choices.map((choice) => (
                      <option key={choice} value={choice}>{choice}</option>
                    ))}
                  </Form.Select>
                ) : (
                  <Form.Control
                    type="text"
                    placeholder="Type your answer..."
                    value={answers.find((a) => a.questionText === q.questionText)?.answer || ""}
                    onChange={(e) => handleAnswerChange(q.questionText, e.target.value)}
                  />
                )}
                {showFeedback && q.feedback && <p className="text-muted mt-1">Feedback: {q.feedback}</p>}
              </div>
            ))}

            {showFeedback && testData?.generalFeedback && <div className="alert alert-info">{testData.generalFeedback}</div>}
          </>
        )}
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onClose}>Close</Button>
        <Button variant="primary" onClick={handleSubmit} disabled={submitting}>
          {submitting ? "Submitting..." : "Submit"}
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default TestModal;
