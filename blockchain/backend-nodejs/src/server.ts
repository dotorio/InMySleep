import app from './app';
import https from 'https';
import fs from 'fs';
import path from 'path';
import { connectDB } from './db/dbConnection';
import { port, sslKey, sslCert, sslChain } from './config';

const options = {
  key: fs.readFileSync(path.join(__dirname, sslKey)),
  cert: fs.readFileSync(path.join(__dirname, sslCert)),
  ca: fs.readFileSync(path.join(__dirname, sslChain))
}

const startServer = async () => {
  try {
    await connectDB();

    https.createServer(options, app).listen(port, () => {
      console.log('https')
      console.log(`Server is running on port ${port}`);
    })
    
  } catch (err) {
    console.error('Error starting server: ', err);
  }
}

startServer();