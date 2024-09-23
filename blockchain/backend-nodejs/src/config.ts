import dotenv from 'dotenv';

dotenv.config();

export const port = process.env.PORT || '';
export const sslFilename = process.env.SSL_FILENAME || '';
export const sslPassword = process.env.SSL_PASSWORD || '';
export const dbHost = process.env.DB_HOST || '';
export const dbUser = process.env.DB_USER || '';
export const dbPassword = process.env.DB_PASSWORD || ''; 
export const dbName = process.env.DB_NAME || '';
export const web3Provider = process.env.WEB3_PROVIDER || '';
export const contractAddress = process.env.CONTRACT_ADDRESS || '';
export const jwtSecret = process.env.JWT_SECRET || '';