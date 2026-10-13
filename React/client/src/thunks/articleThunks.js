import { createAsyncThunk } from "@reduxjs/toolkit";
import api from "../api/api";
import { getToken } from "../utils/token";

export const getArticles = createAsyncThunk('articles/getArticles', async () => {
  const response = await api.get('articles');
  return response.data;
});

export const getArticle = createAsyncThunk('articles/getArticle', async (id) => {
  const response = await api.get(`articles/${id}`);
  return response.data;
});

export const createArticle = createAsyncThunk('articles/createArticle', async (article) => {
  const response = await api.post('articles', article, {
    headers: { Authorization: `Bearer ${getToken()}` }
  });
  return response.data;
});

// update
export const updateArticle = createAsyncThunk('articles/updateArticle', async ({ id, data }) => {
  const response = await api.put(`articles/${id}`, data, {
    headers: { Authorization: `Bearer ${getToken()}` }
  });
  return response.data;
});

// delete
export const deleteArticle = createAsyncThunk('articles/deleteArticle', async (id) => {
  const response = await api.delete(`articles/${id}`, {
    headers: { Authorization: `Bearer ${getToken()}` }
  });
  return response.data;
});
