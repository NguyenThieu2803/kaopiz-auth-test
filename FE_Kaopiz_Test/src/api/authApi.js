import axios from "axios";

const API_URL = "http://localhost:5034/api/Auth"; 

// Tạo axios instance với config chung
const apiClient = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Interceptor để tự động thêm token vào header
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Interceptor để handle lỗi 401 (token hết hạn)
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export const register = async (data) => {
  return apiClient.post('/register', data);
};

export const login = async (data) => {
  return apiClient.post('/login', data);
};

export const logout = async () => {
  const response = await apiClient.post('/logout');
  localStorage.removeItem('token');
  return response;
};