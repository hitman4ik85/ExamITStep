import { createAsyncThunk } from "@reduxjs/toolkit";
import api from "../api/api";
import { getToken } from "../utils/token";

export const login = createAsyncThunk('login', async (user, { rejectWithValue }) => {
  try {
    const response = await api.post('login', user);
    return response.data;
  }
  catch (err) {
    const data = err?.response?.data;
    const msg = (data && (data.message || data.error)) || data || 'Login failed';
    return rejectWithValue(msg);
  }
});

export const register = createAsyncThunk('register', async (user) => {
  const response = await api.post('register', user);
  return response.data;
});

export const getAuthUser = createAsyncThunk('getAuthUser', async () => {
  const response = await api.post('authUser', null, {
    headers: { Authorization: `Bearer ${getToken()}` }
  });
  return response.data;
});

// редагування профілю
export const updateProfile = createAsyncThunk('updateProfile', async (data) => {
  const response = await api.put('profile', data, {
    headers: { Authorization: `Bearer ${getToken()}` }
  });
  return response.data;
});