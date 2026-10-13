import mongoose from "mongoose";

const schema = new mongoose.Schema({
    articleId: { 
        type: mongoose.Schema.Types.ObjectId, 
        ref: 'Article', 
        required: true 
    },
    userId: { type: mongoose.Schema.Types.ObjectId, 
        ref: 'User', 
        required: true 
    },
    userName: { 
        type: String 
    },
    content: { 
        type: String, 
        required: true 
    },
    createdAt: { 
        type: Date, 
        default: Date.now 
    },
    updatedAt: {
        type: Date,
        default: Date.now
    }
});

const Comment = mongoose.model('Comment', schema);
export default Comment;
