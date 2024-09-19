import { Router } from 'express';
import { loginWallet } from '../controllers/walletController';

const router = Router();

router.post('/auth', loginWallet);

export default router;