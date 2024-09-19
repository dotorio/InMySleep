import { Router } from 'express';
import { getMetadata } from '../controllers/nftController';

const router = Router();

router.get('/:tokenId', getMetadata);

export default router;