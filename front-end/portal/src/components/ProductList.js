import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { getProducts } from "../services/api";
import Card from "react-bootstrap/Card";
import CardGroup from "react-bootstrap/CardGroup";
import Col from "react-bootstrap/Col";
import Row from "react-bootstrap/Row";

const ProductList = () => {
  const [product, setProduct] = useState([]);

  useEffect(() => {
    const fetchProducts = async () => {
      const { data } = await getProducts();
      setProduct(data.data);
    };
    fetchProducts();
  }, []);

  return (
    <div className="container my-5">
      <h2>Product List</h2>
      <Row xs={1} md={4} className="g-4">
        {product.map((product, index) => (
          <Col key={index}>
            <Card className="mx-2">
              <Card.Img
                variant="top"
                src={`https://localhost:44366${product.images?.split(":")[0]}`}
              />
              <Card.Body>
                <Card.Title>{product.title}</Card.Title>
                <Card.Text>{product.description}</Card.Text>
              </Card.Body>
              <Card.Footer>
              <Link to={`/product/${product.id}`} className="btn btn-warning">
                  View Details
                </Link>
              </Card.Footer>
            </Card>
          </Col>
        ))}
      </Row>
    </div>
  );
};

export default ProductList;
