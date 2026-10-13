import { Button, Form, Input } from 'antd';
import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { updateProfile } from '../../thunks/authThunk';

const Profile = () => {
    const [form] = Form.useForm();
    const dispatch = useDispatch();
    const auth = useSelector((state) => state.auth);

    // коли підтягнеться користувач — підставляємо значення у форму
    useEffect(() => {
        if (auth.user) {
            form.setFieldsValue({
                name: auth.user.name || '',
                email: auth.user.email || '',
                password: '',
                confirmPassword: ''
            });
        }
    }, [auth.user, form]);

    const onFinish = (values) => {
        const { confirmPassword, ...data } = values; // confirm не відправляємо
        dispatch(updateProfile(data));
    };

    return (
        <div>
            <h1>Profile</h1>
            <Form
                name="profile"
                form={form}
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
                    ]}
                >
                    <Input />
                </Form.Item>

                <Form.Item
                    label="Password"
                    name="password"
                    rules={[
                        // пароль НЕ обов’язковий при зміні профілю
                        ({ getFieldValue }) => ({
                            validator(_, value) {
                                if (!value) return Promise.resolve();
                                if (value.length < 8) return Promise.reject(new Error('Min 8 characters'));
                                const ok = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$/.test(value);
                                return ok ? Promise.resolve() : Promise.reject(new Error('One number, one lowercase, one uppercase'));
                            }
                        })
                    ]}
                >
                    <Input.Password />
                </Form.Item>

                <Form.Item
                    label="Confirm"
                    name="confirmPassword"
                    dependencies={['password']}
                    rules={[
                        ({ getFieldValue }) => ({
                            validator(_, value) {
                                const pwd = getFieldValue('password');
                                // якщо пароль не вводили — confirm не потрібен
                                if (!pwd && !value) return Promise.resolve();
                                if (pwd && !value) return Promise.reject(new Error('Please confirm your Password!'));
                                if (value !== pwd) return Promise.reject(new Error('Passwords do not match'));
                                return Promise.resolve();
                            }
                        })
                    ]}
                >
                    <Input.Password />
                </Form.Item>

                <Form.Item label={null}>
                    <Button type="primary" htmlType="submit">
                        Save
                    </Button>
                </Form.Item>
            </Form>
        </div>
    );
};

export default Profile;
