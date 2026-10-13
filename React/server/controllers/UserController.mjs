import User from "../models/User.mjs";
import bcrypt from "bcrypt";

// список користувачів (для адміна)
export const getUsers = async (req, res) => {
    try {
        const users = await User.find().select("-password").sort({ createdAt: -1 });
        res.send(users);
    } catch (e) {
        res.status(400).send(e.message);
    }
};

// оновлення користувача (для адміна)
export const updateUser = async (req, res) => {
    try {
        const { id } = req.params;
        const data = { ...req.body };

        // унікальність email (якщо передано email)
        if (data.email) {
            const existsEmail = await User.findOne({ email: data.email, _id: { $ne: id } });
            if (existsEmail) {
                return res.status(400).json({ message: "Email already exists" });
            }
        }

        // унікальність name (якщо передано name)
        if (data.name) {
            const existsName = await User.findOne({ name: data.name, _id: { $ne: id } });
            if (existsName) {
                return res.status(400).json({ message: "Name already exists" });
            }
        }

        if (data.password) {
            data.password = await bcrypt.hash(data.password, 10);
        }

        data.updatedAt = new Date();

        const user = await User.findByIdAndUpdate(
            id,
            data,
            { new: true, projection: { password: 0 } }
        );

        if (!user) return res.status(404).json({ message: "User not found" });

        res.send(user);
    } catch (e) {
        res.status(400).send(e.message);
    }
};

// зміна ролі (для адміна)
export const setUserRole = async (req, res) => {
    try {
        const { id } = req.params;
        const { role } = req.body;
        const user = await User.findByIdAndUpdate(
            id,
            { role, updatedAt: new Date() },
            { new: true, projection: { password: 0 } }
        );
        if (!user) return res.status(404).json({ message: "User not found" });
        res.send(user);
    } catch (e) {
        res.status(400).send(e.message);
    }
};

// видалення користувача (для адміна)
export const deleteUser = async (req, res) => {
    try {
        const { id } = req.params;
        const deleted = await User.findByIdAndDelete(id);
        if (!deleted) return res.status(404).json({ message: "User not found" });
        res.send({ ok: true });
    } catch (e) {
        res.status(400).send(e.message);
    }
};
