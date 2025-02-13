import React, { useState } from "react";
import { login, verify2FA } from "../api/userApi";
import { useAuth } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [twoFactorCode, setTwoFactorCode] = useState("");
  const [step, setStep] = useState(1);
  const [error, setError] = useState("");
  const [userId, setUserId] = useState("");
  const { login: setAuthData } = useAuth();
  const navigate = useNavigate();

  const handleLoginSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await login({ email, password });
      if (response.userId) {
        setUserId(response.userId);
        setStep(2);
      }
    } catch (err) {
      setError("Giriş başarısız, bilgilerinizi kontrol edin.");
    }
  };

  const handle2FASubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const token = await verify2FA({ userId, twoFactorCode });
      if (token) {
        setAuthData(token, userId);
        navigate("/");
      }
    } catch (err) {
      setError("2FA doğrulaması başarısız, kodu kontrol edin.");
    }
  };

  return (
    <div className="container mt-5">
      {step === 1 && (
        <>
          <h2>Giriş Yap</h2>
          <form onSubmit={handleLoginSubmit}>
            <div className="mb-3">
              <label className="form-label">Email</label>
              <input
                type="email"
                className="form-control"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </div>
            <div className="mb-3">
              <label className="form-label">Şifre</label>
              <input
                type="password"
                className="form-control"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
            </div>
            {error && <div className="alert alert-danger">{error}</div>}
            <button type="submit" className="btn btn-primary">Giriş Yap</button>
            <button type="button" className="btn btn-link" onClick={() => navigate("/register")}>
              Kayıt Ol
            </button>
          </form>
        </>
      )}

      {step === 2 && (
        <>
          <h2>2FA Doğrulama</h2>
          <form onSubmit={handle2FASubmit}>
            <div className="mb-3">
              <label className="form-label">2FA Kodu</label>
              <input
                type="text"
                className="form-control"
                value={twoFactorCode}
                onChange={(e) => setTwoFactorCode(e.target.value)}
                required
              />
            </div>
            {error && <div className="alert alert-danger">{error}</div>}
            <button type="submit" className="btn btn-primary">Doğrula</button>
          </form>
        </>
      )}
    </div>
  );
};

export default Login;
