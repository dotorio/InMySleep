import dotenv from 'dotenv';

dotenv.config();

export const port = process.env.PORT || 3000;
export const dbHost = process.env.DB_HOST || 'localhost';
export const dbUser = process.env.DB_USER || 'root';
export const dbPassword = process.env.DB_PASSWORD || 'root'; 
export const dbName = process.env.DB_NAME || 'nft';
export const contractAddress = process.env.CONTRACT_ADDRESS || '';
export const jwtSecret = process.env.JWT_SECRET || 'secret';