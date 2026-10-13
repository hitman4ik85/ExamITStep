import express from 'express'
import dbConnection from './db/db.mjs'
import cors from 'cors'
import { createArticle, deleteArticle, getArticle, getArticles, updateArticle } from './controllers/ArticleController.mjs'
import { getAuthUser, login, register, updateProfile } from './controllers/AuthController.mjs'
import { createComment, deleteComment, getCommentsByArticle, updateComment } from './controllers/CommentController.mjs'
import { getUsers, updateUser, setUserRole, deleteUser } from './controllers/UserController.mjs'
import auth from './middleware/auth.mjs'
import isAdmin from './middleware/isAdmin.mjs'
import User from './models/User.mjs' // для /check-name та /check-email

const app = express()
const port = 5000

dbConnection.on('connected', () => console.log('Connected to MongoDB'))
dbConnection.on('error', () => console.log('Connected to MongoDB error'))

app.use(express.json())
app.use(cors());

app.get('/', (req, res) => {
  res.send('Hello!')
})

// перевірка унікальності name (для фронтенд-валідації)
app.get('/check-name', async (req, res) => {
  try {
    const exists = await User.findOne({ name: req.query.name });
    res.json({ exists: !!exists });
  } catch (e) {
    res.status(400).send(e.message);
  }
});

// перевірка унікальності email (для фронтенд-валидації)
app.get('/check-email', async (req, res) => {
  try {
    const exists = await User.findOne({ email: req.query.email });
    res.json({ exists: !!exists });
  } catch (e) {
    res.status(400).send(e.message);
  }
});

// articles
app.get('/articles', getArticles)
app.get('/articles/:id', getArticle)
app.post('/articles', auth, isAdmin, createArticle)
app.put('/articles/:id', auth, isAdmin, updateArticle)
app.delete('/articles/:id', auth, isAdmin, deleteArticle)

// auth
app.post('/register', register)
app.post('/login', login)
app.post('/authUser', auth, getAuthUser)

// profile
app.put('/profile', auth, updateProfile)

// comments
app.get('/articles/:id/comments', getCommentsByArticle)
app.post('/articles/:id/comments', auth, createComment)
// перевірки власника/адміна усередині контролера
app.put('/comments/:id', auth, updateComment)
app.delete('/comments/:id', auth, deleteComment)

// users (admin only)
app.get('/users', auth, isAdmin, getUsers)
app.put('/users/:id', auth, isAdmin, updateUser)
app.put('/users/:id/role', auth, isAdmin, setUserRole)
app.delete('/users/:id', auth, isAdmin, deleteUser)

app.listen(port, () => {
  console.log(`Example app listening on http://localhost:${port}`)
})
