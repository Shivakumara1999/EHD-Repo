import React, { useState } from "react";
import axios from "axios";
// import "../../global.css";
// import jwtDecode from "jwt-decode";
import { useNavigate } from "react-router-dom";
import Meta from "antd/es/card/Meta";
import { Avatar, Button, Card, Form, Input, message } from "antd";
import {
  EyeInvisibleOutlined,
  EyeTwoTone,
  FacebookOutlined,
  GoogleOutlined,
  LinkedinOutlined,
  UserOutlined,
} from "@ant-design/icons";
import Password from "antd/es/input/Password";

const Login: React.FC = () => {
  const [form] = Form.useForm();

  const onFinish = (values: any) => {
    console.log("Received values:", values);
    message.success("Login successful!");
  };

  const validateEmail = async (rule: any, value: string) => {
    if (!value || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
      throw new Error("Please enter a valid email address");
    }
  };

  return (
    <div style={{ display: "flex", height: "100%" }}>
      <div className="image-container" style={{ flex: "70%" }}>
        <img
          src="./images/FcBusinessman.png"
          className="logo-image"
          alt="logo"
          style={{ width: "100%", height: "100%", objectFit: "cover" }}
        />
      </div>

      <div className="form-container" style={{ flex: "30%", padding: "20px" }}>
        <Card>
          <Form form={form} onFinish={onFinish} className="login-container">
            <div className="form-block">
              <div className="loginDiv">
                <b>Helpdesk Joy</b>
              </div>

              <div style={{ marginTop: "3%" }}>
                <div style={{ display: "flex", alignItems: "center" }}>
                  <div className="loginDiv">
                    --------------LOGIN--------------
                  </div>
                </div>
              </div>

              <Form.Item
                label="Email Address"
                name="email"
                rules={[
                  {
                    required: true,
                    message: "Please enter your email address",
                  },
                  { validator: validateEmail },
                ]}
              >
                <Input
                  style={{ height: "6vh" }}
                  type="email"
                  placeholder="example@email.com"
                />
              </Form.Item>

              <Form.Item
                label="Password"
                name="password"
                rules={[
                  { required: true, message: "Please enter your password" },
                ]}
              >
                <Input.Password
                  style={{ height: "6vh" }}
                  placeholder="input password"
                />
              </Form.Item>

              <Form.Item>
                <Button
                  type="primary"
                  htmlType="submit"
                  style={{
                    width: "100%",
                    marginTop: "15%",
                    backgroundColor: "#fc8019",
                    color: "white",
                  }}
                >
                  Login
                </Button>
              </Form.Item>
              <div style={{ display: "flex", justifyContent: "space-evenly" }}>
                <Button>
                  <FacebookOutlined />
                  FaceBook
                </Button>
                <Button>
                  <GoogleOutlined />
                  Google
                </Button>
              </div>
            </div>
          </Form>
        </Card>
      </div>
    </div>
  );
};

export default Login;
