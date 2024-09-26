import { connectDB } from './dbConnection';
import { FieldPacket } from 'mysql2';

export const getEquipeedSkinByUserId = async (userId: string): Promise<any> => {
    try {
        const conn = await connectDB();
        const query = `
        SELECT m.*
        FROM user_skin u
        JOIN metadata m
            ON (JSON_UNQUOTE(JSON_EXTRACT(m.attributes, '$.character')) = 'bear'
                AND JSON_UNQUOTE(JSON_EXTRACT(m.attributes, '$.color')) = u.bear_skin)
            OR (JSON_UNQUOTE(JSON_EXTRACT(m.attributes, '$.character')) = 'rabbit'
                AND JSON_UNQUOTE(JSON_EXTRACT(m.attributes, '$.color')) = u.rabbit_skin)
        WHERE u.user_id = ?;`;
        const [rows]: [any[], FieldPacket[]] = await conn.query(query, [userId]);
        return rows;
    } catch (error) {
        console.error('Error getting skin by user ID and character:', error);
        throw error;
    }
}

export const getSkinListByUserId = async (userId: string): Promise<any> => {
    try {
        const conn = await connectDB();
        const [rows]: [any[], FieldPacket[]] = await conn.query('SELECT * FROM metadata WHERE id IN (SELECT metadata_id FROM user_easter_egg WHERE user_id = ?)', [userId]);
        return rows;
    } catch (error) {
        console.error('Error getting skin list by user ID:', error);
        throw error;
    }
}

export const putEquipSkin = async (userId: string, character: string, color: string): Promise<any> => {
    try {
        const conn = await connectDB();
        const query = `
        UPDATE user_skin
        SET ${character}_skin = ?
        WHERE user_id = ?;`;
        const [rows]: [any[], FieldPacket[]] = await conn.query(query, [color, userId]);
        return rows;
    } catch (error) {
        console.error('Error equipping skin:', error);
        throw error;
    }
}