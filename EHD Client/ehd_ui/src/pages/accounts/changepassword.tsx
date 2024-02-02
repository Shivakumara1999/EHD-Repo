import { useEffect, useState } from "react";
import {
  Avatar,
  Button,
  Card,
  Dropdown,
  Form,
  Input,
  Menu,
  Modal,
  Tooltip,
  message,
} from "antd";
import axios from "axios";

import { RiLockPasswordLine, RiLogoutCircleLine } from "react-icons/ri";
import { IoLogOutOutline } from "react-icons/io5";
import { IoMdArrowDropdown } from "react-icons/io";
import { LuUser2 } from "react-icons/lu";
import "../global.css";
// import { useNavigate } from "react-router-dom";
interface UserData {
  user_Name: string;
}

const UserAndChangePassword = () => {
  const [userProfile, setUserProfile] = useState<UserData | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [verificationSent, setVerificationSent] = useState(false);
  const [verificationCode, setVerificationCode] = useState("");
  const [changePasswordEnabled, setChangePasswordEnabled] = useState(false);

  const Email = sessionStorage.getItem("email");
  //   const navigate = useNavigate();

  useEffect(() => {
    if (Email) {
      axios
        .get(`https://localhost:7120/api/Login/UserProfile?mail_id=${Email}`)
        .then((response: any) => {
          setUserProfile(response.data);
        })
        .catch((error: any) => {
          console.error(error.message);
        });
    }
  }, [Email]);

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

  const showModal = () => {
    setIsModalOpen(true);
  };

  const handleCancel = () => {
    setIsModalOpen(false);
  };

  return (
    <div>
      <div className="content">
        
      <Dropdown
      overlay={
        <Menu style={{ minWidth: "200px" }}>
          <Menu.Item className="changePassword"
            key="change_password"
            icon={<RiLockPasswordLine />}
           
            onClick={showModal}
          >
            Change Password
          </Menu.Item>
          <Menu.Item className="logout"
            key="logout"
            icon={<IoLogOutOutline />}
            
          >
            Logout
          </Menu.Item>
        </Menu>
      }
      trigger={["click"]}
    >
      <Tooltip title="" className="tooltip-content">
        <div className="tooltip-avatar">
          <h2>
            <LuUser2 className="usericon" />
          </h2>
          <p>
            <strong>User</strong>
            {/* {userProfile && userProfile.user_Name} */}
          </p>
          <IoMdArrowDropdown className="arrowdropicon"/>
        </div>
      </Tooltip>
    </Dropdown>
        
      </div>
      <Modal
        className="custom-modal"
        title=""
        open={isModalOpen}
        footer={[]}
        onCancel={handleCancel}
        width={300}
        style={{ marginTop: "6%" }}
      >
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
      </Modal>
    </div>
  );
};

export default UserAndChangePassword;
