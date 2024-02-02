import React, { useState } from "react";
import { Form, Input, Button, message } from "antd";

const ChangePassword = () => {
  const [verificationSent, setVerificationSent] = useState(false);
  const [verificationCode, setVerificationCode] = useState("");
  const [changePasswordEnabled, setChangePasswordEnabled] = useState(false);

  const onFinish = (values:any) => {
    if (verificationSent) {
      // Handle email verification logic
      if (values.verificationCode === verificationCode) {
        setChangePasswordEnabled(true);
        message.success("Email verified successfully!");
      } else {
        message.error("Invalid verification code. Please try again.");
      }
    } else {
      // Handle sending verification code logic (e.g., through API)
      // For demo purposes, assuming verification code is '123456'
      setVerificationCode("123456");
      setVerificationSent(true);
      message.success("Verification code sent to your email.");
    }
  };

  return (
    <Form
      name="changePassword"
      initialValues={{ remember: true }}
      onFinish={onFinish}
    >
      {verificationSent ? (
        <>
          <Form.Item
            label="Verification Code"
            name="verificationCode"
            rules={[
              {
                required: true,
                message:
                  "Please input the verification code sent to your email!",
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Old Password"
            name="oldPassword"
            rules={[
              {
                required: true,
                message: "Please input your old password!",
              },
            ]}
          >
            <Input.Password />
          </Form.Item>
          <Form.Item
            label="New Password"
            name="newPassword"
            rules={[
              {
                required: true,
                message: "Please input your new password!",
              },
            ]}
          >
            <Input.Password />
          </Form.Item>
        </>
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
          {verificationSent
            ? "Verify and Change Password"
            : "Send Verification Code"}
        </Button>
      </Form.Item>

      {changePasswordEnabled && (
        <Form.Item>
          <Button type="primary" htmlType="button">
            Change Password
          </Button>
        </Form.Item>
      )}
    </Form>
  );
};

export default ChangePassword;
