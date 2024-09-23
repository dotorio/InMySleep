import app from './app';
import https from 'https';
import fs from 'fs';
import path from 'path';
import { connectDB } from './db/dbConnection';
import { port, sslFilename, sslPassword } from './config';

const options = {
  pfx: fs.readFileSync(path.join(__dirname, sslFilename)),
  passphrase: sslPassword,
}

const startServer = async () => {
  try {
    await connectDB();

    https.createServer(options, app).listen(port, () => {
      console.log(`Server is running on port ${port}`);
    })
    
  } catch (err) {
    console.error('Error starting server: ', err);
  }
}

startServer();