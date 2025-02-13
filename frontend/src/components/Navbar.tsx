import { useState } from "react";
import { useAuth } from "../context/AuthContext";
import { Container, Navbar, Nav, Button } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import CreateBookModal from "./CreateBookModal";
import JoinGameModal from "./JoinGameModal";

const CustomNavbar = () => {
  const { token, logout } = useAuth();
  const [showBookModal, setShowBookModal] = useState(false);
  const [showGameModal, setShowGameModal] = useState(false);
  const navigate = useNavigate();

  return (
    <>
      <Navbar expand="lg" className="shadow-sm" style={{ backgroundColor: "#1976D2" }}>
        <Container>
          <Navbar.Brand href="/" className="fw-bold text-white">📚 BookApp</Navbar.Brand>
          <Navbar.Toggle aria-controls="navbarNav" />
          <Navbar.Collapse id="navbarNav">
            <Nav className="me-auto">
              <Nav.Link href="/" className="text-white fw-semibold">🏠 Home</Nav.Link>
              {token && <Nav.Link href="/profile" className="text-white fw-semibold">👤 Profile</Nav.Link>}
            </Nav>
            {!token ? (
              <Button variant="light" className="fw-bold text-primary shadow-sm mx-2" onClick={() => navigate("/login")}>
                🔑 Login
              </Button>
            ) : (
              <>
                <Button variant="danger" className="fw-bold shadow-sm mx-2" onClick={logout}>
                  🚪 Logout
                </Button>
                <Button variant="light" className="fw-bold text-primary shadow-sm" onClick={() => setShowBookModal(true)}>
                  ➕ Create Book
                </Button>
                <Button variant="success" className="fw-bold text-white shadow-sm mx-2" onClick={() => setShowGameModal(true)}>
                  🎮 Join Game
                </Button>
              </>
            )}
          </Navbar.Collapse>
        </Container>
      </Navbar>
      
      {showBookModal && <CreateBookModal onClose={() => setShowBookModal(false)} />}
      {showGameModal && <JoinGameModal onClose={() => setShowGameModal(false)} />}
    </>
  );
};

export default CustomNavbar;
