import { connectDB } from './dbConnection';

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