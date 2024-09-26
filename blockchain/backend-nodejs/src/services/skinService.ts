import { getEquipeedSkinByUserId, getSkinListByUserId } from '../db/skinRepository';

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