import { ethers } from 'ethers';
import jwt from 'jsonwebtoken';
import { saveWalletToDB } from '../db/userRepository';
import { jwtSecret } from '../config';

// export const tmpUserLogin = async (userId: string, password: string): Promise<any> => {
//     try {
//         const account = await tmpLogin(userId, password);
//         return account
//     } catch (error) {
//         console.error('Error logging in user:', error);
//         throw error;
//     }
// }

export const verifyMetaMaskSignature = async (address: string, signature: string, message: string): Promise<boolean> => {
    try {
        const recoveredAddress = ethers.verifyMessage(message, signature);
        return recoveredAddress.toLowerCase() === address.toLowerCase();
    } catch (error) {
        console.error('Error verifying signature:', error);
        throw error;
    }
}

export const saveWallet = async (address: string, username: string): Promise<void> => {
    try {
        await saveWalletToDB(address, username); 
        console.log('Saving wallet:', address);
    } catch (error) {
        console.error('Error saving wallet:', error);
        throw error;
    }
}

export const generateJWT = async (address: string): Promise<string> => {
    const token = jwt.sign({ address }, jwtSecret as string, { expiresIn: '1h' });
    return token;
}

export const verifyJWT = async (token: string): Promise<string | jwt.JwtPayload> => {
    const decoded = jwt.verify(token, jwtSecret as string);
    return decoded;
}