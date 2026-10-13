import { createSlice } from "@reduxjs/toolkit";
import { getAuthUser, login, updateProfile } from "../thunks/authThunk";
import { getToken, removeToken, setToken } from "../utils/token";

const initialState = {
    user: {},
    token: null,
    status: 'idle',
    error: null
};

const authSlice = createSlice({
    name: "auth",
    initialState,
    reducers: {
        logout: (state) => {
            state.user = {};
            state.token = null;
            state.status = 'idle';
            state.error = null;
            removeToken();
        }
    },
    extraReducers: (builder) => {
        builder
            .addCase(login.pending, (state) => {
                state.status = 'loading';
            })
            .addCase(login.fulfilled, (state, action) => {
                state.status = 'success';
                state.user = action.payload.user;
                state.token = action.payload.token;
                setToken(action.payload.token);
            })
            .addCase(login.rejected, (state, action) => {
                state.status = 'failed';
                state.error = action.payload || action.error.message;
            })

            .addCase(getAuthUser.pending, (state) => {
                state.status = 'loading';
            })
            .addCase(getAuthUser.fulfilled, (state, action) => {
                state.status = 'success';
                state.user = action.payload;
                state.token = getToken();
            })
            .addCase(getAuthUser.rejected, (state, action) => {
                state.status = 'failed';
                state.error = action.error.message;
                state.user = {};
                state.token = null;
                removeToken();
            })

            .addCase(updateProfile.pending, (state) => {
                state.status = 'loading';
            })
            .addCase(updateProfile.fulfilled, (state, action) => {
                state.status = 'success';
                state.user = action.payload;
            })
            .addCase(updateProfile.rejected, (state, action) => {
                state.status = 'failed';
                state.error = action.error.message;
            });
    },
})

export const { logout } = authSlice.actions;
export default authSlice.reducer;
