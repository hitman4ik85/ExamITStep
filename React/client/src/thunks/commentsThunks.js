import { createAsyncThunk } from "@reduxjs/toolkit";
import api from "../api/api";
import { getToken } from "../utils/token";

export const fetchComments = createAsyncThunk('comments/fetchByArticle', async (articleId) => {
    const res = await api.get(`articles/${articleId}/comments`);
    return res.data;
});

export const createComment = createAsyncThunk('comments/create', async ({ articleId, content }) => {
    const res = await api.post(`articles/${articleId}/comments`, { content }, {
        headers: { Authorization: `Bearer ${getToken()}` }
    });
    return res.data;
});

export const updateComment = createAsyncThunk('comments/update', async ({ id, content }) => {
    const res = await api.put(`comments/${id}`, { content }, {
        headers: { Authorization: `Bearer ${getToken()}` }
    });
    return res.data;
});

export const deleteComment = createAsyncThunk('comments/delete', async (id) => {
    const res = await api.delete(`comments/${id}`, {
        headers: { Authorization: `Bearer ${getToken()}` }
    });
    return res.data;
});
