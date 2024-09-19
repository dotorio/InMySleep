import { Request, Response } from 'express';
import { verifyMetaMaskSignature, generateJWT } from '../services/walletService';

export const loginWallet = async (req: Request, res: Response) => {
    const { address, signature, message } = req.body;
    try {
        console.log('address', address);
        console.log('signature', signature);
        console.log('message', message);
        const isValid = await verifyMetaMaskSignature(address, signature, message);
        if (isValid) {
            const token = await generateJWT(address);
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