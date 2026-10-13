import { createSlice } from "@reduxjs/toolkit";
import { createComment, deleteComment, fetchComments, updateComment } from "../thunks/commentsThunks";

const initialState = {
    list: [],
    status: 'idle',
    error: null
};

const commentsSlice = createSlice({
    name: 'comments',
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder
            // fetch
            .addCase(fetchComments.pending, (state) => { state.status = 'loading'; })
            .addCase(fetchComments.fulfilled, (state, action) => {
                state.status = 'success';
                state.list = action.payload;
            })
            .addCase(fetchComments.rejected, (state, action) => {
                state.status = 'failed';
                state.error = action.error.message;
            })
            // create
            .addCase(createComment.fulfilled, (state, action) => {
                state.list = [...state.list, action.payload];
            })
            // update
            .addCase(updateComment.fulfilled, (state, action) => {
                state.list = state.list.map(c => c._id === action.payload._id ? action.payload : c);
            })
            // delete
            .addCase(deleteComment.fulfilled, (state, action) => {
                state.list = state.list.filter(c => c._id !== action.meta.arg);
            });
    }
});

export default commentsSlice.reducer;
