import { Router } from 'express';
import { getEquippedSkin, getSkinList } from '../controllers/skinController';

const router = Router();

router.get('/equipped', getEquippedSkin);
router.get('/list', getSkinList);

export default router;