import axios from "axios";

const apiUrl = "https://localhost:44366/api"; 

// Lofin
export const login = async (username, password) => {
  try {
    const response = await axios.post(`${apiUrl}/login`, {
      userName: username,
      password: password,
    });
    return response.data;
  } catch (error) {
    console.error("Error logging in:", error);
    throw error;
  }
};

// Lấy danh sách sản phẩm
export const getProducts = async () => {
  try {
    const token = localStorage.getItem("token");
    const response = await axios.get(`${apiUrl}/product/get-all`, {
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching products:", error);
    throw error;
  }
};

// Thêm sản phẩm mới
export const addProduct = async (productData) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axios.post(`${apiUrl}/product/add`, productData, {
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "multipart/form-data"
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error adding product:", error);
    throw error;
  }
};

// Sửa sản phẩm
export const updateProduct = async (productData) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axios.put(`${apiUrl}/product/update`, productData, {
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "multipart/form-data"
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error updating product:", error);
    throw error;
  }
};

// Xóa sản phẩm
export const deleteProduct = async (id) => {
  try {
    const token = localStorage.getItem("token");
    await axios.delete(`${apiUrl}/product/delete/${id}`, {
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    });
  } catch (error) {
    console.error("Error deleting product:", error);
    throw error;
  }
};
