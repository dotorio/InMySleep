import { Router } from 'express';
import { mint, burn, updateMetadata } from '../controllers/contractController';

const router = Router();

router.post('/mint', mint);
router.post('/burn', burn);
router.put('/:tokenId', updateMetadata);

export default router;
