import React from 'react';
import './Header.css';
import { NavLink } from 'react-router-dom';
import { Avatar, Dropdown, Space } from 'antd';
import { LogoutOutlined, UserOutlined } from '@ant-design/icons';
import useAuth from '../../hooks/useAuth';
import { useDispatch, useSelector } from 'react-redux';
import { logout } from '../../slices/authSlice';

const Header = () => {
  const isAuth = useAuth();
  const dispatch = useDispatch();
  const role = useSelector((s) => s.auth?.user?.role);

  const items = [
    { label: <NavLink to="/profile">Profile</NavLink>, key: '0' },
    { type: 'divider' },
    { label: <Space size={15} onClick={() => dispatch(logout())}><LogoutOutlined />Logout</Space>, key: 'logout' },
  ];

  return (
    <header className="header">
      <nav className="main-menu">
        <NavLink to="/">Home</NavLink>
        {isAuth && role === 'admin' && <NavLink to="/users">Users</NavLink>}
        {isAuth && role === 'admin' && <NavLink to="/articles/create">Create Articles</NavLink>}
      </nav>

      <nav className="auth-menu">
        {isAuth ? (
          <Dropdown menu={{ items }} trigger={['click']}>
            <Avatar icon={<UserOutlined />} style={{ backgroundColor: '#87d068' }} />
          </Dropdown>
        ) : (
          <>
            <NavLink to="/login">Login</NavLink>
            <NavLink to="/register">Register</NavLink>
          </>
        )}
      </nav>
    </header>
  );
};

export default Header;
