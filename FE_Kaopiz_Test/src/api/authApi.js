import axios from "axios";

const API_URL = "http://localhost:5034/"; // URL BE của bạn

export const register = async (data) => {
  return axios.post(`${API_URL}/register`, data);
};

export const login = async (data) => {
  return axios.post(`${API_URL}/login`, data);
};
