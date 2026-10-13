import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { deleteArticle, getArticles } from '../../thunks/articleThunks';
import Loader from '../../components/Loader/Loader';
import { NavLink, useNavigate } from 'react-router-dom';
import './Articles.css';
import { EditOutlined, DeleteOutlined } from '@ant-design/icons';

const Articles = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const articles = useSelector((state) => state.articles);
  const role = useSelector((s) => s.auth?.user?.role);

  const [q, setQ] = useState('');

  useEffect(() => {
    dispatch(getArticles());
  }, [dispatch]);

  if (articles.status === 'loading') return <Loader />;
  if (articles.status === 'failed') return <h1>{articles.error}</h1>;

  const filtered = q.trim()
    ? articles.data.filter(a =>
      (a.title || '').toLowerCase().includes(q.trim().toLowerCase())
    )
    : articles.data;

  return (
    <div>
      <h2>Articles</h2>

      <div style={{ margin: '8px 0 12px' }}>
        <input
          type="text"
          value={q}
          onChange={(e) => setQ(e.target.value)}
          placeholder="Search articles..."
          style={{ minWidth: 260, padding: '6px 8px' }}
        />
      </div>

      <table className="articles-table">
        <thead>
          <tr>
            <th className="col-title">Title</th>
            <th className="col-date">Created</th>
            <th className="col-date">Updated</th>
            {role === 'admin' && <th className="col-actions">Actions</th>}
          </tr>
        </thead>
        <tbody>
          {filtered.map((article) => {
            const created = article.createdAt || article.date;
            const createdText = created ? new Date(created).toLocaleDateString('uk-UA') : '';
            const updatedText = article.updatedAt ? new Date(article.updatedAt).toLocaleDateString('uk-UA') : '';
            return (
              <tr key={article._id}>
                <td className="col-title">
                  <NavLink to={`/articles/${article._id}`} className="articles-link">
                    {article.title}
                  </NavLink>
                </td>
                <td className="col-date">{createdText}</td>
                <td className="col-date">{updatedText}</td>
                {role === 'admin' && (
                  <td className="col-actions">
                    <EditOutlined
                      className="row-icon"
                      title="Edit article"
                      onClick={() => navigate(`/articles/${article._id}/edit`)}
                    />
                    <DeleteOutlined
                      className="row-icon danger"
                      title="Delete article"
                      onClick={async () => {
                        if (window.confirm('Are you sure you want to delete this article?')) {
                          await dispatch(deleteArticle(article._id));
                          dispatch(getArticles());
                        }
                      }}
                    />
                  </td>
                )}
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
};

export default Articles;