import mysql from 'mysql2/promise';
import { dbHost, dbUser, dbPassword, dbName } from '../config';

let connection: mysql.Connection | null = null;

export const connectDB = async (): Promise<mysql.Connection> => {
  if (!connection) {
      connection = await mysql.createConnection({
        host: dbHost,
        user: dbUser,
        password: dbPassword,
        database: dbName
      });
      console.log('MySQL Connected');
  }
  return connection;
};
