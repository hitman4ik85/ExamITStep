import { Button, Form, Input, notification } from 'antd';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { login } from '../../thunks/authThunk';
import { useNavigate, NavLink } from 'react-router-dom';

const Login = () => {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const auth = useSelector((s) => s.auth);
    const [api, contextHolder] = notification.useNotification();

    const onFinish = async (values) => {
        try {
            await dispatch(login(values)).unwrap();
            navigate('/');
        } catch (err) {
            const description = typeof err === 'string' ? err : 'Unknown error';
            api.error({
                message: 'Login Error',
                description,
                placement: 'topRight'
            });
        }
    };

    return (
        <div>
            <h1>Login</h1>
            <Form
                name="login"
                labelCol={{ span: 8 }}
                wrapperCol={{ span: 16 }}
                style={{ maxWidth: 600 }}
                onFinish={onFinish}
                autoComplete="off"
            >
                <Form.Item
                    label="Email"
                    name="email"
                    rules={[
                        { required: true, message: 'Please input your Email!' },
                        { type: 'email', message: 'Invalid email format' }
                    ]}
                >
                    <Input />
                </Form.Item>

                <Form.Item
                    label="Password"
                    name="password"
                    rules={[
                        { required: true, message: 'Please input your Password!' }
                    ]}
                >
                    <Input.Password />
                </Form.Item>

                <Form.Item label={null}>
                    <div style={{ display: 'flex', alignItems: 'center', gap: '12px' }}>
                        <Button type="primary" htmlType="submit" loading={auth.status === 'loading'}>
                            Submit
                        </Button>
                        <NavLink to="/register">Register</NavLink>
                    </div>
                </Form.Item>
            </Form>
            {contextHolder}
        </div>
    );
};

export default Login;
