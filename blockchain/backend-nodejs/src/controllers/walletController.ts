import { Request, Response } from 'express';
import { verifyMetaMaskSignature, saveWallet, generateJWT } from '../services/walletService';

// export const tmpLogin = async (req: Request, res: Response) => {
//     const { username, password } = req.body;
//     try {
//         const account = await tmpUserLogin(username, password);
//         res.status(200).json(account);
//     } catch (error) {
//         if (error instanceof Error) {
//             res.status(500).json({ error: error.message });
//         } else {
//             res.status(500).json({ error: 'An error occurred' });
//         }
//     }
// }

export const loginWallet = async (req: Request, res: Response) => {
    const { address, signature, message, username } = req.body;
    try {
        const isValid = await verifyMetaMaskSignature(address, signature, message);
        if (isValid) {
            const token = await generateJWT(address);
            await saveWallet(address, username);
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