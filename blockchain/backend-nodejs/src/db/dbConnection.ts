import mysql from 'mysql2/promise';
import { dbHost, dbUser, dbPassword, dbName } from '../config';

// let connection: mysql.Connection | null = null;

const pool = mysql.createPool({
  host: dbHost,
  user: dbUser,
  password: dbPassword,
  database: dbName,
  waitForConnections: true,
  connectionLimit: 10,
  queueLimit: 0
});

export default pool;

// export const connectDB = async (): Promise<mysql.Connection> => {
//   if (!connection) {
//       connection = await mysql.createConnection({
//         host: dbHost,
//         user: dbUser,
//         password: dbPassword,
//         database: dbName
//       });
//       console.log('MySQL Connected');
//   }
//   return connection;
// };
