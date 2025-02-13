import axios from "axios";

// const API_URL = "http://localhost/api/User"; 
const API_URL = "http://localhost:5256/api/User"; 

export const register = async (user: { 
  email: string; 
  password: string; 
  name: string; 
  surname: string; 
  roles?: string[]; 
}) => {
  const response = await axios.post(`${API_URL}/register`, user, {
    headers: { "Content-Type": "application/json" },
  });
  return response.data;
};

export const login = async (credentials: { email: string; password: string }) => {
  const response = await axios.post(`${API_URL}/login`, credentials, {
    headers: { "Content-Type": "application/json" },
  });
  return response.data; 
};

export const verify2FA = async (data: { userId: string; twoFactorCode: string }) => {
  const response = await axios.post(`${API_URL}/verify-2fa`, data, {
    headers: { "Content-Type": "application/json" },
  });
  return response.data.token;
};
