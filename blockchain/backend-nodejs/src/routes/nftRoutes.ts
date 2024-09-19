import { Router } from 'express';
import { getMetadata } from '../controllers/nftController';

const router = Router();

router.get('/:tokenId', getMetadata);
// router.get('mynfts', getMyNFTs);

export default router;