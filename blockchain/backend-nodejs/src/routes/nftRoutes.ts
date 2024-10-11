import { Router } from 'express';
import { getMetadata, getMyNFTs } from '../controllers/nftController';

const router = Router();

router.get('/mynfts', getMyNFTs);
router.get('/:tokenId', getMetadata);

export default router;