import { connectDB } from './dbConnection';
import { FieldPacket } from 'mysql2';

// export const tmpLogin = async (userId: string, password: string): Promise<any> => {
//     try {
//         const conn = await connectDB();
//         const [rows]: [any[], FieldPacket[]] = await conn.query('SELECT * FROM user WHERE user_id = ? AND password = ?', [userId, password]);
//         if (rows.length === 0) {
//             throw new Error('Invalid credentials');
//         }
//         console.log('Logged in user:', userId);
//         return rows
//     } catch (error) {
//         console.error('Error logging in user:', error);
//         throw error;
//     }
// }

export const saveWalletToDB = async (address: string, username: string): Promise<void> => {
    try {
        const conn = await connectDB();
        const [rows] = await conn.query('UPDATE user SET wallet_address = ? where username = ?', [address, username]);
        console.log('Saved wallet to DB:', rows);
    } catch (error) {
        console.error('Error saving wallet to DB:', error);
        throw error;
    }
}

export const getSkinByUserId = async (userId: string): Promise<any> => {
    try {
        const conn = await connectDB();
        console.log(userId)
        const [rows]: [any[], FieldPacket[]] = await conn.query('SELECT * FROM user_skin WHERE user_id = ?', [userId]);
        console.log('Got skin by user ID:', rows);
        return rows[0];
    } catch (error) {
        console.error('Error getting skin by user ID and character:', error);
        throw error;
    }
}

export const getSkinListByUserId = async (userId: string): Promise<any> => {
    try {
        const conn = await connectDB();
        const [rows]: [any[], FieldPacket[]] = await conn.query('SELECT * FROM user_easter_egg WHERE user_id = ?', [userId]);
        return rows[0];
    } catch (error) {
        console.error('Error getting skin list by user ID:', error);
        throw error;
    }
}