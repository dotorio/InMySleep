import { Router } from 'express';
import { mint, burn, getMetadata } from '../controllers/contractController';

const router = Router();

router.post('/mint', mint);
router.post('/burn', burn);
router.get('/:tokenId', getMetadata);

export default router;
