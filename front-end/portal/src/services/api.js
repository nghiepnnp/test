import axios from "axios";

const BASE_URL = "https://localhost:44366/api";

export const register = async (userData) =>
  await axios.post(`${BASE_URL}/register`, userData);

export const login = async (credentials) =>
  await axios.post(`${BASE_URL}/login`, credentials);

export const getProducts = async () =>
  await axios.get(`${BASE_URL}/product/get-all`);

export const getProductById = async (id) =>
  await axios.get(`${BASE_URL}/product/get/${id}`);

export const addComment = async (commentData) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axios.post(`${BASE_URL}/comment/add`, commentData, {
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error updating product:", error);
    throw error;
  }
};
