// import { Request, Response } from 'express';
// import { uploadFileToIPFS } from '../services/ipfsService';

// export const uploadToIPFS = async (req: Request, res: Response) => {
//   const { file } = req.body;  // 파일을 받아온다고 가정
//   try {
//     const cid = await uploadFileToIPFS(file);
//     res.status(200).json({ cid });
//   } catch (error) {
//     res.status(500).json({ error: error.message });
//   }
// };