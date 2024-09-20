import { Request, Response } from 'express';
import { verifyMetaMaskSignature, saveWallet, generateJWT, verifyJWT, tmpUserLogin } from '../services/walletService';

export const tmpLogin = async (req: Request, res: Response) => {
    const { userId, password } = req.body;
    try {
        const account = await tmpUserLogin(userId, password);
        res.status(200).json(account);
    } catch (error) {
        if (error instanceof Error) {
            res.status(500).json({ error: error.message });
        } else {
            res.status(500).json({ error: 'An error occurred' });
        }
    }
}

export const loginWallet = async (req: Request, res: Response) => {
    const { address, signature, message, userId } = req.body;
    try {
        console.log('address', address);
        console.log('signature', signature);
        console.log('message', message);
        const isValid = await verifyMetaMaskSignature(address, signature, message);
        if (isValid) {
            const token = await generateJWT(address);
            await saveWallet(address, userId);
            res.status(200).json({ token });
        } else {
            res.status(401).json({ error: 'Unauthorized' });
        }
    } catch (error) {
        if (error instanceof Error) {
            res.status(500).json({ error: error.message });
        } else {
            res.status(500).json({ error: 'An error occurred' });
        }
    }
}