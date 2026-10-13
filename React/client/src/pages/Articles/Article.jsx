import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useParams, useNavigate } from 'react-router-dom';
import Loader from '../../components/Loader/Loader';
import { getArticle, deleteArticle } from '../../thunks/articleThunks';
import { createComment, deleteComment, fetchComments, updateComment } from '../../thunks/commentsThunks';
import useAuth from '../../hooks/useAuth';
import './Article.css';
import { EditOutlined, DeleteOutlined, SaveOutlined, RollbackOutlined, SendOutlined } from '@ant-design/icons';

const Article = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const { current, currentStatus, currentError } = useSelector((state) => state.articles);
  const comments = useSelector((s) => s.comments);
  const isAuth = useAuth();
  const role = useSelector((s) => s.auth?.user?.role);
  const userId = useSelector((s) => s.auth?.user?._id);

  const [text, setText] = useState('');
  const [editId, setEditId] = useState(null);
  const [editText, setEditText] = useState('');
  const [cQuery, setCQuery] = useState('');

  useEffect(() => {
    dispatch(getArticle(id));
    dispatch(fetchComments(id));
  }, [dispatch, id]);

  if (currentStatus === 'loading') return <Loader />;
  if (currentStatus === 'failed') return <h1>{currentError}</h1>;
  if (!current) return null;

  const created = current.createdAt || current.date;
  const createdText = created ? new Date(created).toLocaleString('uk-UA') : '';
  const updatedText = current.updatedAt ? new Date(current.updatedAt).toLocaleString('uk-UA') : '';

  const showActionsColumn = Boolean(userId) || role === 'admin';

  const filteredComments = cQuery.trim()
    ? comments.list.filter(c =>
      (c.userName || 'User').toLowerCase().includes(cQuery.trim().toLowerCase())
    )
    : comments.list;

  return (
    <div>
      {/* Заголовок + адмінські дії над статтею */}
      <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
        <h1 style={{ margin: 0 }}>{current.title}</h1>
        {role === 'admin' && (
          <span>
            <EditOutlined
              className="row-icon"
              title="Edit article"
              onClick={() => navigate(`/articles/${id}/edit`)}
            />
            <DeleteOutlined
              className="row-icon danger"
              title="Delete article"
              onClick={async () => {
                if (window.confirm('Are you sure you want to delete this article?')) {
                  await dispatch(deleteArticle(id));
                  navigate('/');
                }
              }}
            />
          </span>
        )}
      </div>

      {/* контент статті (із ReactQuill) */}
      <div
        className="article-content"
        style={{ margin: '12px 0' }}
        dangerouslySetInnerHTML={{ __html: current.content || '' }}
      />

      {createdText && <div>Created: {createdText}</div>}
      {updatedText && <div>Updated: {updatedText}</div>}

      <hr style={{ margin: '20px 0' }} />
      <h3>Comments</h3>

      <div style={{ margin: '8px 0 8px' }}>
        <input
          type="text"
          value={cQuery}
          onChange={(e) => setCQuery(e.target.value)}
          placeholder="Search comments by author..."
          style={{ minWidth: 260, padding: '6px 8px' }}
        />
      </div>

      <table className="comments-table">
        <thead>
          <tr>
            <th className="col-author">Author</th>
            <th className="col-content">Content</th>
            <th className="col-date">Created</th>
            <th className="col-date">Updated</th>
            {showActionsColumn && <th className="col-actions">Actions</th>}
          </tr>
        </thead>
        <tbody>
          {filteredComments.map((c) => {
            const createdC = c.createdAt ? new Date(c.createdAt).toLocaleString('uk-UA') : '';
            const updatedC = c.updatedAt ? new Date(c.updatedAt).toLocaleString('uk-UA') : '';
            const isEditing = editId === c._id;
            const isOwner = userId && String(userId) === String(c.userId);
            const canModify = Boolean(isOwner || role === 'admin');

            return (
              <tr key={c._id}>
                <td className="col-author">{c.userName || 'User'}</td>
                <td className="col-content">
                  {isEditing ? (
                    <div className="edit-row">
                      <input
                        className="edit-input"
                        value={editText}
                        onChange={(e) => setEditText(e.target.value)}
                        placeholder="Edit comment..."
                      />
                      {canModify && (
                        <>
                          <SaveOutlined
                            className="row-icon"
                            title="Save comment"
                            onClick={async () => {
                              const v = editText.trim();
                              if (!v) return;
                              await dispatch(updateComment({ id: c._id, content: v }));
                              setEditId(null);
                              setEditText('');
                            }}
                          />
                          <RollbackOutlined
                            className="row-icon"
                            title="Cancel editing"
                            style={{ color: 'red' }}
                            onClick={() => {
                              setEditId(null);
                              setEditText('');
                            }}
                          />
                        </>
                      )}
                    </div>
                  ) : (
                    c.content
                  )}
                </td>
                <td className="col-date">{createdC}</td>
                <td className="col-date">{updatedC}</td>
                {showActionsColumn && (
                  <td className="col-actions actions-cell">
                    {isEditing ? (
                      <span className="muted">Редагування…</span>
                    ) : canModify ? (
                      <>
                        <EditOutlined
                          className="row-icon"
                          title="Edit comment"
                          onClick={() => {
                            setEditId(c._id);
                            setEditText(c.content);
                          }}
                        />
                        <DeleteOutlined
                          className="row-icon danger"
                          title="Delete comment"
                          onClick={() => dispatch(deleteComment(c._id))}
                        />
                      </>
                    ) : (
                      <span className="muted">—</span>
                    )}
                  </td>
                )}
              </tr>
            );
          })}
        </tbody>
      </table>

      {isAuth && (
        <div className="add-comment">
          <input
            className="add-input"
            value={text}
            onChange={(e) => setText(e.target.value)}
            placeholder="Your comment..."
            onKeyDown={async (e) => {
              if (e.key === 'Enter' && !e.shiftKey && text.trim()) {
                e.preventDefault();
                await dispatch(createComment({ articleId: id, content: text.trim() }));
                setText('');
              }
            }}
          />
          <SendOutlined
            className="row-icon send"
            title="Send comment"
            onClick={async () => {
              if (!text.trim()) return;
              await dispatch(createComment({ articleId: id, content: text.trim() }));
              setText('');
            }}
          />
        </div>
      )}
    </div>
  );
};

export default Article;