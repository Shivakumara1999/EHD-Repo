import React, { useState, useEffect } from "react";
import { Layout, Menu } from "antd";

import { Link, Route, Routes, useNavigate } from "react-router-dom";
import "../global.css";
import Sider from "antd/es/layout/Sider";
import { Content } from "antd/es/layout/layout";

import { MdOutlineGridView } from "react-icons/md";
import { FaFileAlt } from "react-icons/fa";
import { HiOutlineViewList, HiUsers } from "react-icons/hi";
import { FaRegFileAlt } from "react-icons/fa";
import { FcDataConfiguration } from "react-icons/fc";
import { BsTicketDetailed } from "react-icons/bs";
import UserAndChangePassword from "../pages/accounts/changepassword";
interface MySidebarProps {
  role: string;
}

const Sidebar: React.FC<MySidebarProps> = ({ role }) => {
  const [collapsed, setCollapsed] = useState(false);
  const [selectedMenuItem, setSelectedMenuItem] = useState<string | null>(
    localStorage.getItem("selectedMenuItem") || "1"
  );
  const navigate = useNavigate();

  const handleMenuClick = (key: string) => {
    setSelectedMenuItem(key);
    localStorage.setItem("selectedMenuItem", key);
  };
  useEffect(() => {
    
    setSelectedMenuItem(localStorage.getItem("selectedMenuItem") || "1");
  }, []);


  const getMenuItems = () => {
    switch (role) {
      case "user":
        return [
          <Menu.Item
            key="1"
            icon={<MdOutlineGridView  />}
            onClick={() => handleMenuClick("1")}
          >
            <Link to="/overview">Overview</Link>
          </Menu.Item>,
          <Menu.Item
            key="2"
            icon={<FaFileAlt  />}
            onClick={() => handleMenuClick("2")}
          >
            <Link to="/createTicket">Create Ticket</Link>
          </Menu.Item>,
          <Menu.Item
            key="3"
            icon={<HiOutlineViewList  />}
            onClick={() => handleMenuClick("3")}
          >
            View Ticket History
          </Menu.Item>,
        ];
      case "admin":
        return [
          <Menu.Item
            key="1"
            icon={<MdOutlineGridView />}
            onClick={() => handleMenuClick("1")}
          >
            <Link to="/overview">Dashboard</Link>
          </Menu.Item>,
          <Menu.Item
            key="2"
            icon={<FaRegFileAlt  />}
            onClick={() => handleMenuClick("2")}
          >
            <Link to="/createTicket">Escalations</Link>
          </Menu.Item>,
          <Menu.Item
            key="3"
            icon={<HiUsers  />}
            onClick={() => handleMenuClick("3")}
          >
         Employees
          </Menu.Item>,
          <Menu.Item
          key="4"
          icon={<FcDataConfiguration  />}
          onClick={() => handleMenuClick("3")}
        >
         Configuartion
        </Menu.Item>,
        <Menu.Item
        key="5"
        icon={<FaFileAlt />}
        onClick={() => handleMenuClick("3")}
      >
      Create Ticket
      </Menu.Item>,
      <Menu.Item
      key="6"
      icon={<HiOutlineViewList />}
      onClick={() => handleMenuClick("3")}
    >
     View Ticket History
    </Menu.Item>,
        ];
      case "department":
        return [
          <Menu.Item
            key="1"
            icon={<MdOutlineGridView />}
            onClick={() => handleMenuClick("1")}
          >
            Overview
          </Menu.Item>,
          <Menu.Item
            key="2"
            icon={<BsTicketDetailed  />}
            onClick={() => handleMenuClick("2")}
          >
            Ticketing System
          </Menu.Item>,
          <Menu.Item
            key="3"
            icon={<FaFileAlt />}
            onClick={() => handleMenuClick("3")}
          >
            Create Ticket
          </Menu.Item>,
          <Menu.Item
            key="4"
            icon={<HiOutlineViewList />}
            onClick={() => handleMenuClick("4")}
          >
            View Ticket History
          </Menu.Item>,
        ];
      default:
        return [];
    }
  };

  const getContentComponent = () => {
    switch (role) {
      case "user":
        return (
          <Routes>
            <Route path="/overview" element={<div>Content for Overview</div>} />
            <Route
              path="/create-ticket"
              element={<div>Content for Create Ticket</div>}
            />
            <Route
              path="/ticket-history"
              element={<div>Content for View Ticket History</div>}
            />
          </Routes>
        );
      case "department":
        return (
          <Routes>
            <Route path="/overview" element={<div>Content for Overview</div>} />
            <Route
              path="/ticket-system"
              element={<div>Content for Ticketing System</div>}
            />
            <Route
              path="/create-ticket"
              element={<div>Content for Create Ticket</div>}
            />
            <Route
              path="/ticket-history"
              element={<div>Content for View Ticket History</div>}
            />
          </Routes>
        );
      default:
        return null;
    }
  };


  return (
    <Layout style={{ minHeight: "100vh" }}>
      <Sider
        theme="dark"
        collapsible
        collapsed={collapsed}
        onCollapse={(value) => setCollapsed(value)}
        className="sidemenus"
      >
        <div className="container">
          <img
            src="./Images/JoyLogo.png"
            alt="Logo"
            className={`images ${collapsed ? "collapsed" : ""}`}
          />
        </div>
        <hr className="line" />
        <Menu
          mode="vertical"
          selectedKeys={[selectedMenuItem || "1"]}
          defaultSelectedKeys={["1"]}
          className="menus"
        >
          {getMenuItems()}
        </Menu>
      </Sider>
      <Layout>
        <Content>
        <UserAndChangePassword/>
          {getContentComponent()}
          </Content>
      </Layout>
    </Layout>
  );
};

export default Sidebar;
