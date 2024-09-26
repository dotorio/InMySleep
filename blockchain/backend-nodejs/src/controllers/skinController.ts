import { Request, Response } from 'express';
import { findEquippedSkin, findSkinList, updateEquipSkin } from '../services/skinService';

export const getEquippedSkin = async (req: Request, res: Response) => {
    const userId = req.query.userId as string;
    try {
        const curSkins = await findEquippedSkin(userId);
        res.status(200).json(curSkins);
    } catch (error) {
        console.error('Error getting equipped skin:', error);
        res.status(500).send('Error getting equipped skin');
    }
}

export const getSkinList = async (req: Request, res: Response) => {
    const userId = req.query.userId as string;
    try {
        const skinList = await findSkinList(userId);
        res.status(200).json(skinList);
    } catch (error) {
        console.error('Error getting skin list:', error);
        res.status(500).send('Error getting skin list');
    }
}

export const putEquipSkin = async (req: Request, res: Response) => {
    const { userId, character, color } = req.body;
    try {
        const result = await updateEquipSkin(userId, character, color);
        res.status(200).json(result);
    } catch (error) {
        console.error('Error equipping skin:', error);
        res.status(500).send('Error equipping skin');
    }
}
