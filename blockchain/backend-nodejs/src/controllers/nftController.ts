import { Request, Response } from 'express';
import { verifyJWT } from '../services/walletService';
import { getNFTMetadata, getNFTs } from '../services/nftService';

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

export const getMyNFTs = async (req: Request, res: Response) => {
  res.set('Cache-Control', 'no-store');
  const address = req.query.address as string;
  const token = req.query.token as string;
  try {
    if (!verifyJWT(token)) {
      res.status(403).json({ error: 'Unauthorized' });
      return;
    }
    const nfts = await getNFTs(address);
    res.status(200).json({ nfts });
  } catch (error) {
    if (error instanceof Error) {
      res.status(500).json({ error: error.message });
    } else {
      res.status(500).json({ error: 'An error occurred' });
    }
  }
}