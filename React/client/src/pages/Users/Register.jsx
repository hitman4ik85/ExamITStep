import { Button, Form, Input, notification } from 'antd';
import React from 'react';
import { useDispatch } from 'react-redux';
import { register } from '../../thunks/authThunk';
import { useNavigate, NavLink } from 'react-router-dom';

const Register = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [api, contextHolder] = notification.useNotification();

  const onFinish = async (values) => {
    const { confirmPassword, ...data } = values;
    try {
      await dispatch(register(data)).unwrap();
      navigate('/login');
    } catch (err) {
      const description = typeof err === 'string' ? err : 'Registration failed';
      api.error({
        message: 'Registration Error',
        description,
        placement: 'topRight'
      });
    }
  };

  return (
    <div>
      <h1>Register</h1>
      <Form
        name="register"
        labelCol={{ span: 8 }}
        wrapperCol={{ span: 16 }}
        style={{ maxWidth: 600 }}
        onFinish={onFinish}
        autoComplete="off"
      >
        <Form.Item
          label="Name"
          name="name"
          rules={[
            { required: true, message: 'Please input your Name!' },
            { min: 2, message: 'Name must be at least 2 characters' },
            // async-валідатор унікальності name
            {
              validator: async (_, value) => {
                const v = (value || '').trim();
                if (!v) return Promise.resolve();
                try {
                  const res = await fetch(`http://localhost:5000/check-name?name=${encodeURIComponent(v)}`);
                  const data = await res.json();
                  if (data?.exists) {
                    return Promise.reject(new Error('This name is already taken'));
                  }
                  return Promise.resolve();
                } catch {
                  // якщо бекенд недоступний — не блокуємо форму
                  return Promise.resolve();
                }
              }
            }
          ]}
        >
          <Input />
        </Form.Item>

        <Form.Item
          label="Email"
          name="email"
          rules={[
            { required: true, message: 'Please input your Email!' },
            { type: 'email', message: 'Invalid email format' },
            // async-валідатор унікальності email
            {
              validator: async (_, value) => {
                const v = (value || '').trim();
                if (!v) return Promise.resolve();
                try {
                  const res = await fetch(`http://localhost:5000/check-email?email=${encodeURIComponent(v)}`);
                  const data = await res.json();
                  if (data?.exists) {
                    return Promise.reject(new Error('This email is already taken'));
                  }
                  return Promise.resolve();
                } catch {
                  return Promise.resolve();
                }
              }
            }
          ]}
        >
          <Input />
        </Form.Item>

        <Form.Item
          label="Password"
          name="password"
          rules={[
            { required: true, message: 'Please input your Password!' },
            { min: 8, message: 'Min 8 characters' },
            { pattern: /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$/, message: 'One number, one lowercase, one uppercase' }
          ]}
        >
          <Input.Password />
        </Form.Item>

        <Form.Item
          label="Confirm"
          name="confirmPassword"
          dependencies={['password']}
          rules={[
            { required: true, message: 'Please confirm your Password!' },
            ({ getFieldValue }) => ({
              validator(_, value) {
                if (!value || getFieldValue('password') === value) return Promise.resolve();
                return Promise.reject(new Error('Passwords do not match'));
              }
            })
          ]}
        >
          <Input.Password />
        </Form.Item>

        <Form.Item label={null}>
          <div style={{ display: 'flex', alignItems: 'center', gap: '12px' }}>
            <Button type="primary" htmlType="submit">
              Submit
            </Button>
            <NavLink to="/login">Login</NavLink>
          </div>
        </Form.Item>
      </Form>
      {contextHolder}
    </div>
  );
};

export default Register;
