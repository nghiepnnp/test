import React, { useState, useEffect } from "react";
import { Button, Container, Table } from "react-bootstrap";
import { getProducts, deleteProduct } from "../services/api";
import { useNavigate } from "react-router-dom";
import ProductForm from "./ProductForm";

const ProductList = () => {
  const navigate = useNavigate();
  const [products, setProducts] = useState([]);
  const [selectedProduct, setSelectedProduct] = useState(null);

  useEffect(() => {
    fetchProducts();
  }, []);

  const token = localStorage.getItem("token");
  if (!token) {
    navigate("/login");
  }

  const fetchProducts = async () => {
    try {
      const data = await getProducts();
      setProducts(data.data);
    } catch (error) {
      console.error("Error fetching products:", error);
    }
  };

  const handleDelete = async (id) => {
    try {
      await deleteProduct(id);
      fetchProducts();
      alert("Deleted!");
    } catch (error) {
      alert("Error:", error);
    }
  };

  const handleEdit = (product) => {
    setSelectedProduct(product);
  };

  const handleClose = () => {
    setSelectedProduct(null);
  };

  const formatCurrency = (amount) => {
    return new Intl.NumberFormat("vi-VN").format(amount) + " đ";
  };

  return (
    <div>
      <Container className="my-5">
        <h1 class="text-center">Product list</h1>
        <Button
          className="my-3"
          variant="primary"
          onClick={() => setSelectedProduct({})}
        >
          Add Product
        </Button>
        <Table striped bordered hover>
          <thead>
            <tr>
              <th class="text-center">#</th>
              <th class="text-center">Image</th>
              <th class="text-center">Title</th>
              <th class="text-center">Price</th>
              <th class="text-center">Actions</th>
            </tr>
          </thead>
          <tbody>
            {products.map((product, index) => (
              <tr key={product.id}>
                <td class="text-center align-middle">{index + 1}</td>
                <td class="text-center align-middle">
                  <img src={`https://localhost:44366${product.images?.split(":")[0]}`} 
                    style={{ maxWidth: "100px", height: "auto", border: "1px solid #ddd" }}
                  />
                </td>
                <td class="align-middle">{product.title}</td>
                <td class="align-middle">{formatCurrency(product.price)}</td>
                <td class="text-center align-middle ">
                  <Button variant="warning" onClick={() => handleEdit(product)}>
                    Edit
                  </Button>
                  <Button
                    className="mx-2 btn btn-sm"
                    variant="danger"
                    onClick={() => handleDelete(product.id)}
                  >
                    Delete
                  </Button>
                </td>
              </tr>
            ))}
          </tbody>
        </Table>
      </Container>

      {/* Hiển thị modal thêm hoặc sửa sản phẩm */}
      {selectedProduct && (
        <ProductForm
          product={selectedProduct}
          onClose={handleClose}
          onSave={fetchProducts}
        />
      )}
    </div>
  );
};

export default ProductList;
