import jwt from "jsonwebtoken";
import User from "../models/User.mjs";

export default async function auth(req, res, next) {
    const token = req.headers.authorization?.replace('Bearer ', '');
    if (!token) return res.status(401).json({ message: 'No token' });
    try {
        const payload = jwt.verify(token, 'secret');
        req._id = payload._id;
        const user = await User.findById(payload._id);
        req.user = user;
        next();
    } catch (e) {
        res.status(401).json({ message: 'Invalid token' });
    }
}
