// components/ProductForm.js
import React, { useState, useEffect } from "react";
import { Modal, Button, Form } from "react-bootstrap";
import { addProduct, updateProduct } from "../services/api";

const ProductForm = ({ product, onClose, onSave }) => {
  const [id, setId] = useState("");
  const [title, setTitle] = useState("");
  const [price, setPrice] = useState("");
  const [description, setDescription] = useState("");
  const [images, setImages] = useState([]);

  const handleFileChange = (e) => {
    setImages(Array.from(e.target.files));
  };

  useEffect(() => {
    if (product) {
      setId(product.id);
      setTitle(product.title);
      setPrice(product.price);
      setDescription(product.description);
    }
  }, [product]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    // const productData = { id, title, price, description };

    const productData = new FormData();
    // productData.append("id", id);
    productData.append("title", title);
    productData.append("price", price);
    productData.append("description", description);

    images.forEach((image) => {
        productData.append("RawFiles", image);
    });

    try {
      if (product && Object.keys(product).length > 0) {
        productData.append("id", id);
        await updateProduct(productData);
        alert("Updated!")
      } else {
        console.log(productData)
        await addProduct(productData);
        alert("Created!")
      }
      onSave(); // Sau khi lưu, gọi lại hàm onSave để làm mới danh sách sản phẩm
      onClose();
    } catch (error) {
        alert("Error:", error);
    }
  };

  return (
    <Modal show={true} onHide={onClose}>
      <Modal.Header closeButton>
        <Modal.Title>{product ? "Edit Product" : "Add Product"}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form.Group className="mb-3">
          <Form.Label>Images</Form.Label>
          <Form.Control
            type="file"
            multiple
            onChange={handleFileChange}
            accept="image/*"
          />
          {images.length > 0 && (
            <div className="mt-2">
              <strong>Selected Files:</strong>
              <ul>
                {images.map((file, index) => (
                  <li key={index}>{file.name}</li>
                ))}
              </ul>
            </div>
          )}
        </Form.Group>

        <Form onSubmit={handleSubmit}>
          <Form.Group controlId="formProductTitle">
            <Form.Label>Title</Form.Label>
            <Form.Control
              type="text"
              placeholder=""
              value={title}
              required
              onChange={(e) => setTitle(e.target.value)}
            />
          </Form.Group>

          <Form.Group controlId="formProductPrice">
            <Form.Label>Price</Form.Label>
            <Form.Control
              type="number"
              placeholder=""
              value={price}
              required
              onChange={(e) => setPrice(e.target.value)}
            />
          </Form.Group>

          <Form.Group className="my-2" controlId="formProductDescription">
            <Form.Label>Description</Form.Label>
            <Form.Control
              as="textarea"
              rows={3}
              value={description}
              onChange={(e) => setDescription(e.target.value)}
            />
          </Form.Group>

          <Button variant="primary" type="submit">
            {product ? "Save Changes" : "Add Product"}
          </Button>
        </Form>
      </Modal.Body>
    </Modal>
  );
};

export default ProductForm;
