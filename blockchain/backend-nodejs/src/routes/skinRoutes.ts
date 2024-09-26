import { Router } from 'express';
import { getEquippedSkin, getSkinList, putEquipSkin } from '../controllers/skinController';

const router = Router();

router.get('/equipped', getEquippedSkin);
router.get('/list', getSkinList);
router.put('/equip', putEquipSkin);

export default router;