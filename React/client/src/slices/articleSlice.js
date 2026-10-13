import { createSlice } from "@reduxjs/toolkit";
import { getArticles, getArticle } from "../thunks/articleThunks";

const initialValues = {
  data: [],
  status: 'idle', // idle | loading | success | failed
  error: null,
  current: null,
  currentStatus: 'idle',
  currentError: null
};

const articleSlice = createSlice({
  name: "articles",
  initialState: initialValues,
  extraReducers: (builder) => {
    builder
      // список
      .addCase(getArticles.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(getArticles.fulfilled, (state, action) => {
        state.status = 'success';
        state.data = action.payload;
      })
      .addCase(getArticles.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message;
      })
      // поточна стаття
      .addCase(getArticle.pending, (state) => {
        state.currentStatus = 'loading';
        state.currentError = null;
        state.current = null;
      })
      .addCase(getArticle.fulfilled, (state, action) => {
        state.currentStatus = 'success';
        state.current = action.payload;
      })
      .addCase(getArticle.rejected, (state, action) => {
        state.currentStatus = 'failed';
        state.currentError = action.error.message;
      });
  }
});

export default articleSlice.reducer;
