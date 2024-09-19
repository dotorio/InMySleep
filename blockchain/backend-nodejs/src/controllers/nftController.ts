import { Request, Response } from 'express';
import { getNFTMetadata } from '../services/nftService';

export const getMetadata = async (req: Request, res: Response) => {
  const { tokenId } = req.params;
  try {
    const metadataURI = await getNFTMetadata(Number(tokenId));
    res.status(200).json({ metadataURI });
  } catch (error) {
    if (error instanceof Error) {
      res.status(500).json({ error: error.message });
    } else {
      res.status(500).json({ error: 'An error occurred' });
    }
  }
};