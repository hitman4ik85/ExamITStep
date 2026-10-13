import { createSlice } from "@reduxjs/toolkit";
import { deleteUser, getUsers, setUserRole, updateUser } from "../thunks/userThunks";

const initialState = {
    list: [],
    status: 'idle',
    error: null
};

const usersSlice = createSlice({
    name: 'users',
    initialState,
    reducers: {},
    extraReducers: (b) => {
        b
            .addCase(getUsers.pending, (s) => { s.status = 'loading'; })
            .addCase(getUsers.fulfilled, (s, a) => { s.status = 'success'; s.list = a.payload; })
            .addCase(getUsers.rejected, (s, a) => { s.status = 'failed'; s.error = a.error.message; })

            .addCase(setUserRole.fulfilled, (s, a) => {
                s.list = s.list.map(u => u._id === a.payload._id ? a.payload : u);
            })

            .addCase(updateUser.fulfilled, (s, a) => {
                s.list = s.list.map(u => u._id === a.payload._id ? a.payload : u);
            })

            .addCase(deleteUser.fulfilled, (s, a) => {
                const id = a.meta.arg;
                s.list = s.list.filter(u => u._id !== id);
            });
    }
});

export default usersSlice.reducer;
