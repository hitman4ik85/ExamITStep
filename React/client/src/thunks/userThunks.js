import { createAsyncThunk } from "@reduxjs/toolkit";
import api from "../api/api";
import { getToken } from "../utils/token";

export const getUsers = createAsyncThunk('users/getUsers', async () => {
    const res = await api.get('users', {
        headers: { Authorization: `Bearer ${getToken()}` }
    });
    return res.data;
});

export const setUserRole = createAsyncThunk('users/setUserRole', async ({ id, role }) => {
    const res = await api.put(`users/${id}/role`, { role }, {
        headers: { Authorization: `Bearer ${getToken()}` }
    });
    return res.data;
});

export const updateUser = createAsyncThunk('users/updateUser', async ({ id, data }) => {
    const res = await api.put(`users/${id}`, data, {
        headers: { Authorization: `Bearer ${getToken()}` }
    });
    return res.data;
});

export const deleteUser = createAsyncThunk('users/deleteUser', async (id) => {
    const res = await api.delete(`users/${id}`, {
        headers: { Authorization: `Bearer ${getToken()}` }
    });
    return res.data;
});
