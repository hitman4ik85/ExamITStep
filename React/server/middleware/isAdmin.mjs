import User from '../models/User.mjs';

const isAdmin = async (req, res, next) => {
    try {
        const user = await User.findById(req._id);
        if (user && user.role === 'admin') {
            return next();
        }
        res.status(403).send('Forbidden');
    } catch (e) {
        res.status(401).send('Invalid user');
    }
};

export default isAdmin;
