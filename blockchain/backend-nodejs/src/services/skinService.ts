import { getEquipeedSkinByUserId, getSkinListByUserId, putEquipSkin } from '../db/skinRepository';

export const findEquippedSkin = async (userId: string): Promise<any> => {
    try {
        return await getEquipeedSkinByUserId(userId);
    } catch (error) {
        console.error('Error finding equipped skin:', error);
        throw error
    }
}

export const findSkinList = async (userId: string): Promise<any> => {
    try {
        return await getSkinListByUserId(userId);
    } catch (error) {
        console.error('Error finding skin list:', error);
        throw error
    }
}

export const updateEquipSkin = async (userId: string, character: string, color: string): Promise<any> => {
    try {
        return await putEquipSkin(userId, character, color);
    } catch (error) {
        console.error('Error inserting equipped skin:', error);
        throw error
    }
}