import User from "../models/User.mjs"
import bcrypt from "bcrypt"
import jwt from "jsonwebtoken"

export const register = async (req, res) => {
    try {
        // валідація: email унікальний
        const exists = await User.findOne({ email: req.body.email });
        if (exists) return res.status(400).json({ message: 'Email already exists' });

        // валідація: name унікальний (додано)
        const existsName = await User.findOne({ name: req.body.name });
        if (existsName) return res.status(400).json({ message: 'Name already exists' });

        const user = new User(req.body)
        await user.save()

        const { password, ...safe } = user.toObject()
        res.send(safe)
    }
    catch (e) {
        res.status(400).send(e.message)
    }
}

export const login = async (req, res) => {
    try {
        const user = await User.findOne({ email: req.body.email })
        if (!user) { return res.status(400).json({ message: 'User not found' }) }

        const validPassword = await bcrypt.compare(req.body.password, user.password)
        if (!validPassword) { return res.status(400).json({ message: 'Invalid password' }) }

        const token = jwt.sign({ _id: user._id }, 'secret', { expiresIn: '1d' })
        const { password, ...safe } = user.toObject()
        res.send({ user: safe, token })
    }
    catch (e) {
        res.status(400).send(e.message)
    }
}

export const getAuthUser = async (req, res) => {
    try {
        const user = await User.findById(req._id).select('-password')
        res.send(user)
    }
    catch (e) {
        res.status(400).send(e.message)
    }
}

// функція редагування профілю (email, name, password)
export const updateProfile = async (req, res) => {
    try {
        const data = { ...req.body };
        if (data.password) {
            data.password = await bcrypt.hash(data.password, 10);
        }
        data.updatedAt = new Date();
        const user = await User.findByIdAndUpdate(
            req._id,
            data,
            { new: true, projection: { password: 0 } }
        );
        res.send(user);
    }
    catch (e) {
        res.status(400).send(e.message);
    }
}
