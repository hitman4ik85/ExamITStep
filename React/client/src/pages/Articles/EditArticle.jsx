import React, { useEffect, useState } from 'react';
import { Form, Input } from 'antd';
import { useDispatch, useSelector } from 'react-redux';
import { getArticle, updateArticle } from '../../thunks/articleThunks';
import { useNavigate, useParams } from 'react-router-dom';
import Loader from '../../components/Loader/Loader';

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

const EditArticle = () => {
    const [form] = Form.useForm();
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const { id } = useParams();

    const { current, currentStatus, currentError } = useSelector((s) => s.articles);

    const [content, setContent] = useState('');

    useEffect(() => {
        dispatch(getArticle(id));
    }, [dispatch, id]);

    useEffect(() => {
        if (current) {
            form.setFieldsValue({
                title: current.title || '',
                content: current.content || '',
            });
            setContent(current.content || '');
        }
    }, [current, form]);

    if (currentStatus === 'loading') return <Loader />;
    if (currentStatus === 'failed') return <h1>{currentError}</h1>;
    if (!current) return null;

    const onFinish = async (values) => {
        const data = { ...values, content };
        await dispatch(updateArticle({ id, data }));
        navigate(`/articles/${id}`);
    };

    return (
        <div>
            <h1>Edit article</h1>
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

                <Form.Item label={null}>
                    <SaveOutlined
                        className="row-icon"
                        title="Save article"
                        onClick={() => form.submit()}
                    />
                    <RollbackOutlined
                        className="row-icon"
                        title="Cancel editing"
                        style={{ color: 'red' }}
                        onClick={() => navigate(`/articles/${id}`)}
                    />
                </Form.Item>
            </Form>
        </div>
    );
};

export default EditArticle;
