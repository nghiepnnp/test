import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { getProductById, addComment } from "../services/api";
import { HubConnectionBuilder } from "@microsoft/signalr";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import Image from "react-bootstrap/Image";
import Alert from "react-bootstrap/Alert";

const ProductDetail = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [product, setProduct] = useState({});
  const [comments, setComments] = useState([]);
  const [newComment, setNewComment] = useState("");

  const token = localStorage.getItem("token");
  const userName = localStorage.getItem("username");

  useEffect(() => {
    const fetchProduct = async () => {
      try {
        const { data } = await getProductById(id);
        console.log(data);
        setProduct(data.data);
        setComments(data.data.comments || []);
      } catch (error) {
        console.error("Error fetching product:", error);
      }
    };
    fetchProduct();

    const connection = new HubConnectionBuilder()
      .withUrl("https://localhost:44366/commentHub")
      .build();

    connection.start();
    connection.on("ReceiveComment", (data) => {
      console.log(data);
      setComments((prevComments) => {
        if (prevComments.some((comment) => comment.id === data.newComment.id)) {
          return prevComments; 
        }
        return [data.newComment, ...prevComments];
      });
    });    
  }, [id]);

  const handleAddComment = async (e) => {
    e.preventDefault();
    if (!newComment.trim()) {
      alert("Comment cannot be empty!");
      return;
    }

    try {
      const data = await addComment({
        productId: product.id,
        username: userName,
        content: newComment,
      });
      setComments([data.data, ...comments]);
      setNewComment("");
    } catch (error) {
      console.error("Error adding comment:", error);
    }
  };

  const formatCurrency = (amount) => {
    return new Intl.NumberFormat("vi-VN").format(amount) + " đ";
  };

  const formatDateTime = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleString("en-GB", {
      weekday: "short", // optional, to include the weekday
      year: "numeric",
      month: "2-digit",
      day: "2-digit",
      hour: "2-digit",
      minute: "2-digit",
      second: "2-digit",
      hour12: false, // 24-hour clock
    });
  };

  return (
    <div className="container my-5">
      <Container style={{ "max-width": "70%" }}>
        {userName ? (
          <h2 className="text-danger">Hi, {userName}</h2>
        ) : (
          <div></div>
        )}
        <Row>
          <Col xs={5}>
            <Image
              src={`https://localhost:44366${product.images?.split(":")[0]}`}
              width={300}
            />
          </Col>
          <Col>
            <h4>Tên sản phẩm: {product.title}</h4>
            <p>Mô tả sản phẩm: {product.description}</p>
            <p>Giá: {formatCurrency(product.price)}</p>
          </Col>
        </Row>
        <h4 className="mt-5">Comments</h4>
        {/* Form thêm bình luận */}
        {token ? (
          <form onSubmit={handleAddComment}>
            <div className="mb-3">
              <label htmlFor="comment" className="form-label">
                Add a Comment
              </label>
              <textarea
                className="form-control"
                id="comment"
                placeholder="Enter your comment here"
                value={newComment}
                onChange={(e) => setNewComment(e.target.value)}
              />
            </div>
            <button type="submit" className="btn btn-primary">
              Submit
            </button>
          </form>
        ) : (
          <form>
            <div className="mb-3">
              <label htmlFor="comment" className="form-label">
                Add a Comment
              </label>
              <textarea
                className="form-control"
                id="comment"
                placeholder="Enter your comment here"
                value={newComment}
                disabled
                onChange={(e) => setNewComment(e.target.value)}
              />
            </div>
            <button
              className="btn btn-secondary"
              onClick={() => navigate("/login")}
            >
              Login
            </button>
          </form>
        )}
        <hr />
        {comments.map((comment, index) => (
          <Alert variant="light">
            <Alert.Heading as="p">
              <i>
                {formatDateTime(comment.createdAt)}: <b>{comment.userName}</b>
              </i>
            </Alert.Heading>
            <p>{comment.content}</p>
          </Alert>
        ))}
      </Container>
    </div>
  );
};
export default ProductDetail;
