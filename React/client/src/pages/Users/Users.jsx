import React, { useEffect, useMemo, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Loader from '../../components/Loader/Loader';
import { deleteUser, getUsers, setUserRole, updateUser } from '../../thunks/userThunks';
import './Users.css';
import { EditOutlined, DeleteOutlined, SaveOutlined, RollbackOutlined } from '@ant-design/icons';

const Users = () => {
    const dispatch = useDispatch();
    const { list, status, error } = useSelector((s) => s.users);
    const role = useSelector((s) => s.auth?.user?.role);

    const [editId, setEditId] = useState(null);
    const [editName, setEditName] = useState('');
    const [editEmail, setEditEmail] = useState('');
    const [editPassword, setEditPassword] = useState('');
    const [editRole, setEditRole] = useState('user');
    const [q, setQ] = useState('');

    useEffect(() => {
        dispatch(getUsers());
    }, [dispatch]);

    // фільтрація по email/name
    const filtered = useMemo(() => {
        const s = q.trim().toLowerCase();
        if (!s) return list;
        return list.filter(u =>
            (u.email || '').toLowerCase().includes(s) ||
            (u.name || '').toLowerCase().includes(s)
        );
    }, [list, q]);

    if (role !== 'admin') return <h3>403 Forbidden</h3>;
    if (status === 'loading') return <Loader />;
    if (status === 'failed') return <h1>{error}</h1>;

    const startEdit = (u) => {
        setEditId(u._id);
        setEditName(u.name || '');
        setEditEmail(u.email || '');
        setEditPassword('');
        setEditRole(u.role || 'user');
    };

    const cancelEdit = () => {
        setEditId(null);
        setEditName('');
        setEditEmail('');
        setEditPassword('');
        setEditRole('user');
    };

    const saveRow = async (u) => {
        // валідація формату (мінімальна)
        const nameTrim = editName.trim();
        const emailTrim = editEmail.trim();
        const passTrim = editPassword.trim();

        if (!nameTrim || nameTrim.length < 2) {
            window.alert('Name must be at least 2 characters');
            return;
        }
        const emailOk = /^\S+@\S+\.\S+$/.test(emailTrim);
        if (!emailOk) {
            window.alert('Invalid email format');
            return;
        }
        if (passTrim) {
            if (passTrim.length < 8) {
                window.alert('Password: Min 8 characters');
                return;
            }
            if (!/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$/.test(passTrim)) {
                window.alert('Password must contain one number, one lowercase, one uppercase');
                return;
            }
        }

        // 🔎 перевірка унікальності перед сабмітом (комфорт адміну)
        try {
            const [nameRes, emailRes] = await Promise.all([
                fetch(`http://localhost:5000/check-name?name=${encodeURIComponent(nameTrim)}`),
                fetch(`http://localhost:5000/check-email?email=${encodeURIComponent(emailTrim)}`)
            ]);
            const [nameData, emailData] = await Promise.all([nameRes.json(), emailRes.json()]);

            // якщо ім'я вже зайняте іншим користувачем (дозволяємо залишити своє поточне)
            const nameTakenByOther = nameData?.exists && nameTrim !== (u.name || '');
            if (nameTakenByOther) {
                window.alert('This name is already taken');
                return;
            }

            // якщо email вже зайнятий іншим користувачем (дозволяємо залишити свій поточний)
            const emailTakenByOther = emailData?.exists && emailTrim !== (u.email || '');
            if (emailTakenByOther) {
                window.alert('This email is already taken');
                return;
            }
        } catch {
            // якщо бекенд не відповів — не блокуємо, бо все одно є серверна перевірка
        }

        const data = { name: nameTrim, email: emailTrim };
        if (passTrim) data.password = passTrim;
        await dispatch(updateUser({ id: u._id, data }));

        const currentRole = u.role || 'user';
        if (currentRole !== editRole) {
            await dispatch(setUserRole({ id: u._id, role: editRole }));
        }
        cancelEdit();
    };

    return (
        <div>
            <h2>Users</h2>

            <div style={{ margin: '8px 0 12px' }}>
                <input
                    type="text"
                    value={q}
                    onChange={(e) => setQ(e.target.value)}
                    placeholder="Search users (email / name)..."
                    style={{ minWidth: 300, padding: '6px 8px' }}
                />
            </div>

            <table className="users-table">
                <thead>
                    <tr>
                        <th className="col-email">Email</th>
                        <th className="col-name">Name</th>
                        <th className="col-pass">Password</th>
                        <th className="col-role">Role</th>
                        <th className="col-date">Created</th>
                        <th className="col-date">Updated</th>
                        <th className="col-actions">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {filtered.map((u) => {
                        const created = u.createdAt ? new Date(u.createdAt).toLocaleString('uk-UA') : '';
                        const updated = u.updatedAt ? new Date(u.updatedAt).toLocaleString('uk-UA') : '';
                        const isEditing = editId === u._id;

                        return (
                            <tr key={u._id}>
                                <td className="col-email">
                                    {isEditing ? (
                                        <input
                                            className="edit-input"
                                            value={editEmail}
                                            onChange={(e) => setEditEmail(e.target.value)}
                                            placeholder="email"
                                        />
                                    ) : (
                                        u.email
                                    )}
                                </td>
                                <td className="col-name">
                                    {isEditing ? (
                                        <input
                                            className="edit-input"
                                            value={editName}
                                            onChange={(e) => setEditName(e.target.value)}
                                            placeholder="name"
                                        />
                                    ) : (
                                        u.name || ''
                                    )}
                                </td>
                                <td className="col-pass">
                                    {isEditing ? (
                                        <input
                                            className="edit-input"
                                            type="password"
                                            value={editPassword}
                                            onChange={(e) => setEditPassword(e.target.value)}
                                            placeholder="new password"
                                        />
                                    ) : (
                                        '••••••'
                                    )}
                                </td>
                                <td className="col-role">
                                    {isEditing ? (
                                        <select
                                            value={editRole}
                                            onChange={(e) => setEditRole(e.target.value)}
                                        >
                                            <option value="user">user</option>
                                            <option value="admin">admin</option>
                                        </select>
                                    ) : (
                                        u.role || 'user'
                                    )}
                                </td>
                                <td className="col-date">{created}</td>
                                <td className="col-date">{updated}</td>
                                <td className="col-actions actions-cell">
                                    {isEditing ? (
                                        <>
                                            <SaveOutlined
                                                className="row-icon"
                                                title="Save"
                                                onClick={() => saveRow(u)}
                                            />
                                            <RollbackOutlined
                                                className="row-icon"
                                                title="Cancel"
                                                style={{ color: 'red' }}
                                                onClick={cancelEdit}
                                            />
                                        </>
                                    ) : (
                                        <>
                                            <EditOutlined
                                                className="row-icon"
                                                title="Edit"
                                                onClick={() => startEdit(u)}
                                            />
                                            <DeleteOutlined
                                                className="row-icon danger"
                                                title="Delete"
                                                onClick={async () => {
                                                    if (window.confirm('Are you sure you want to delete this user?')) {
                                                        await dispatch(deleteUser(u._id));
                                                    }
                                                }}
                                            />
                                        </>
                                    )}
                                </td>
                            </tr>
                        );
                    })}
                </tbody>
            </table>
        </div>
    );
};

export default Users;