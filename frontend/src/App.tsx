import { BrowserRouter as Router, Routes, Route, useLocation } from "react-router-dom";
import Navbar from "./components/Navbar";
import BookPage from "./pages/BookPage";
import BookDetail from "./pages/BookDetail";
import Profile from "./pages/Profile";
import Login from "./pages/Login";
import Register from "./pages/Register";
import { AuthProvider } from "./context/AuthContext";
import "./App.css";
import WordsPage from "./pages/WordsPage";
import PopularPage from "./pages/PopularPage";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import GamePage from "./pages/GamePage";

const AppContent = () => {
  const location = useLocation();
  const hideNavbar = location.pathname === "/login" || location.pathname === "/register";
  console.log("[AppContent] Current path:", location.pathname);

  return (
    <>
      {!hideNavbar && <Navbar />}
      <div className="mt-4">
        <ToastContainer position="top-right" autoClose={3000} />
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/" element={<BookPage />} />
          <Route path="/book/:id" element={<BookDetail />} />
          <Route path="/profile" element={<Profile />} />
          <Route path="/words" element={<WordsPage />} />
          <Route path="/popular-books" element={<PopularPage />} />
          <Route path="/game/:roomId" element={<GamePage />} />
        </Routes>
      </div>
    </>
  );
};

const App = () => {
  console.log("[App] Rendering");
  return (
    <AuthProvider>
      <Router>
        <AppContent />
      </Router>
    </AuthProvider>
  );
};

export default App;
