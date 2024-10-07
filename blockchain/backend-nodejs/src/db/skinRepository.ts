// import { connectDB } from './dbConnection';
import pool from './dbConnection';
import { FieldPacket } from 'mysql2';

export const getEquipeedSkinByUserId = async (userId: string): Promise<any> => {
    try {
        const query = `SELECT *
                        FROM metadata
                        WHERE id IN (
                            SELECT bear_skin_metadata FROM user_skin WHERE user_id = ?
                            UNION
                            SELECT rabbit_skin_metadata FROM user_skin WHERE user_id = ?
                        );`;
        const [rows]: [any[], FieldPacket[]] = await pool.query(query, [userId, userId]);
        return rows;
    } catch (error) {
        console.error('Error getting skin by user ID and character:', error);
        throw error;
    }
}

export const getSkinListByUserId = async (userId: string): Promise<any> => {
    try {
        const [rows]: [any[], FieldPacket[]] = await pool.query('SELECT * FROM metadata WHERE id IN (SELECT metadata_id FROM user_easter_egg WHERE user_id = ?)', [userId]);
        return rows;
    } catch (error) {
        console.error('Error getting skin list by user ID:', error);
        throw error;
    }
}

export const putEquipSkin = async (userId: string, character: string, color: string): Promise<any> => {
    try {
        const query = `
        UPDATE user_skin
        SET ${character}_skin_metadata = ?
        WHERE user_id = ?;`;
        const [rows]: [any[], FieldPacket[]] = await pool.query(query, [color, userId]);
        return rows;
    } catch (error) {
        console.error('Error equipping skin:', error);
        throw error;
    }
}