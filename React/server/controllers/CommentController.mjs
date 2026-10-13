import Comment from "../models/Comment.mjs";
import User from "../models/User.mjs";

export const getCommentsByArticle = async (req, res) => {
    try {
        const comments = await Comment
            .find({ articleId: req.params.id })
            .sort({ createdAt: 1 });
        res.send(comments);
    } catch (e) {
        res.status(400).send(e.message);
    }
};

export const createComment = async (req, res) => {
    try {
        const user = await User.findById(req._id);
        const userName = user?.name || user?.email || 'Unknown';

        const comment = new Comment({
            articleId: req.params.id,
            userId: req._id,
            userName,
            content: req.body.content
        });

        await comment.save();
        res.send(comment);
    } catch (e) {
        res.status(400).send(e.message);
    }
};

// автор або адміністратор може редагувати
export const updateComment = async (req, res) => {
    try {
        const c = await Comment.findById(req.params.id);
        if (!c) return res.status(404).send('Comment not found');

        const isOwner = String(c.userId) === String(req._id);
        const isAdmin = req.user?.role === 'admin';   // роль передаємо з мідлвара

        if (!(isOwner || isAdmin)) {
            return res.status(403).send('You can edit only your comment');
        }

        c.content = req.body.content;
        c.updatedAt = new Date();          // <<< додано оновлення дати
        await c.save();
        res.send(c);
    } catch (e) {
        res.status(400).send(e.message);
    }
};

// автор або адміністратор може видаляти
export const deleteComment = async (req, res) => {
    try {
        const c = await Comment.findById(req.params.id);
        if (!c) return res.status(404).send('Comment not found');

        const isOwner = String(c.userId) === String(req._id);
        const isAdmin = req.user?.role === 'admin';

        if (!(isOwner || isAdmin)) {
            return res.status(403).send('You can delete only your comment');
        }

        await c.deleteOne();
        res.send({ ok: true });
    } catch (e) {
        res.status(404).send(e.message);
    }
};