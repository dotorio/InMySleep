import { FieldPacket } from 'mysql2/promise';
import { connectDB } from './dbConnection';

export const tmpLogin = async (userId: string, password: string): Promise<any> => {
    try {
        const conn = await connectDB();
        const [rows]: [any[], FieldPacket[]] = await conn.query('SELECT * FROM users WHERE user_id = ? AND password = ?', [userId, password]);
        if (rows.length === 0) {
            throw new Error('Invalid credentials');
        }
        console.log('Logged in user:', userId);
        return rows
    } catch (error) {
        console.error('Error logging in user:', error);
        throw error;
    }
}

export const saveWalletToDB = async (address: string, userId: string): Promise<void> => {
    try {
        const conn = await connectDB();
        const [rows] = await conn.query('UPDATE users SET wallet_address = ? where user_id = ?', [address, userId]);
        console.log('Saved wallet to DB:', rows);
    } catch (error) {
        console.error('Error saving wallet to DB:', error);
        throw error;
    }
}