import { ethers } from 'ethers';
import jwt from 'jsonwebtoken';
import { jwtSecret } from '../config';

export const verifyMetaMaskSignature = async (address: string, signature: string, message: string): Promise<boolean> => {
    const recoveredAddress = ethers.verifyMessage(message, signature);
    return recoveredAddress.toLowerCase() === address.toLowerCase();
}

export const generateJWT = async (address: string): Promise<string> => {
    const token = jwt.sign({ address }, jwtSecret as string, { expiresIn: '1h' });
    return token;
}