import app from './app';
import { connectDB } from './db/dbConnection';
import { port } from './config';

const startServer = async () => {
  try {
    await connectDB();

    app.get('/', (req, res) => {
      res.send('Hello World!');
    });

    app.listen(port, () => {
      console.log(`Server is running on port ${port}`);
    });
  } catch (err) {
    console.error('Error starting server: ', err);
  }
}

startServer();