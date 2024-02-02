import React, { useState } from "react";
import { Form, Input, Button, message } from "antd";

const ForgotPassword = () => {
  const [otpSent, setOtpSent] = useState(false);
  const [resetEnabled, setResetEnabled] = useState(false);

  const onFinish = (values:any) => {
    if (otpSent) {
      // Handle OTP verification logic
      if (values.otp === "123456") {
        setResetEnabled(true);
        message.success("OTP verified successfully!");
      } else {
        message.error("Invalid OTP. Please try again.");
      }
    } else {
      // Handle sending OTP logic (e.g., through API)
      // For demo purposes, assuming OTP is '123456'
      setOtpSent(true);
      message.success("OTP sent to your email.");
    }
  };

  return (
    <Form
      name="forgotPassword"
      initialValues={{ remember: true }}
      onFinish={onFinish}
    >
      {otpSent ? (
        <Form.Item
          label="OTP"
          name="otp"
          rules={[
            {
              required: true,
              message: "Please input the OTP sent to your email!",
            },
          ]}
        >
          <Input />
        </Form.Item>
      ) : (
        <Form.Item
          label="Email"
          name="email"
          rules={[
            {
              required: true,
              type: "email",
              message: "Please input your email!",
            },
          ]}
        >
          <Input />
        </Form.Item>
      )}

      <Form.Item>
        <Button type="primary" htmlType="submit">
          {otpSent ? "Verify OTP" : "Send OTP"}
        </Button>
      </Form.Item>

      {resetEnabled && (
        <Form.Item>
          <Button type="primary" htmlType="button">
            Reset Password
          </Button>
        </Form.Item>
      )}
    </Form>
  );
};

export default ForgotPassword;
