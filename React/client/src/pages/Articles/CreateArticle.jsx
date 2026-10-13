import React, { useState } from 'react';
import { Form, Input } from 'antd';
import { useDispatch } from 'react-redux';
import { createArticle } from '../../thunks/articleThunks';
import { useNavigate } from 'react-router-dom';

// 🖊️ rich-text редактор
import ReactQuill from 'react-quill-new';
import 'react-quill-new/dist/quill.snow.css';

// іконки
import { SaveOutlined, RollbackOutlined } from '@ant-design/icons';

const quillModules = {
    toolbar: [
        [{ header: [1, 2, 3, false] }],
        ['bold', 'italic', 'underline', 'strike'],
        [{ color: [] }, { background: [] }],
        [{ list: 'ordered' }, { list: 'bullet' }],
        [{ align: [] }],
        ['blockquote', 'code-block'],
        ['clean'],
    ],
};
const quillFormats = [
    'header',
    'bold', 'italic', 'underline', 'strike',
    'color', 'background',
    'list', 'bullet',
    'align',
    'blockquote', 'code-block',
];

const CreateArticle = () => {
    const [form] = Form.useForm();
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const [content, setContent] = useState('');

    const onFinish = async (values) => {
        const data = { ...values, content };
        await dispatch(createArticle(data));
        navigate('/');
    };

    return (
        <div>
            <h1>Create article</h1>
            <Form
                form={form}
                labelCol={{ span: 6 }}
                wrapperCol={{ span: 14 }}
                style={{ maxWidth: 800 }}
                onFinish={onFinish}
                autoComplete="off"
            >
                <Form.Item
                    label="Title"
                    name="title"
                    rules={[
                        { required: true, message: 'Please input Title!' },
                        { min: 3, message: 'Title must be at least 3 characters' },
                    ]}
                >
                    <Input />
                </Form.Item>

                {/* сховане поле, щоб antd валідація знала про content */}
                <Form.Item
                    name="content"
                    hidden
                    rules={[{ required: true, message: 'Please input Description!' }]}
                >
                    <Input />
                </Form.Item>

                <Form.Item label="Description" required>
                    <ReactQuill
                        theme="snow"
                        value={content}
                        onChange={(html) => {
                            setContent(html);
                            form.setFieldsValue({ content: html });
                        }}
                        modules={quillModules}
                        formats={quillFormats}
                    />
                </Form.Item>

                {/* іконки дій у нашому стилі */}
                <Form.Item label={null}>
                    <SaveOutlined
                        className="row-icon"
                        title="Save article"
                        onClick={() => form.submit()}
                    />
                    <RollbackOutlined
                        className="row-icon"
                        title="Cancel creating"
                        style={{ color: 'red' }}
                        onClick={() => navigate('/')}
                    />
                </Form.Item>
            </Form>
        </div>
    );
};

export default CreateArticle;
