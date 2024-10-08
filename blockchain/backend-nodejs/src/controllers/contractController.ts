import { Request, Response } from 'express';
import { mintNFT, burnNFT, updateNFTMetadata } from '../services/contractService';
import { saveMintedNFT, saveBurnedNFT } from '../services/nftService';
import { jsonBigIntStringify } from '../utils/jsonHelpers';
import { privateKey } from '../config';

export const mint = async (req: Request, res: Response) => {
  const { userId, address, tokenUri } = req.body;
  try {
    const receipt = await mintNFT(address, tokenUri, privateKey);
    await saveMintedNFT(receipt, userId, address, tokenUri);
    res.status(200).json(jsonBigIntStringify(receipt));
  } catch (error) {
    if (error instanceof Error) {
      res.status(500).json({ error: error.message });
    } else {
      res.status(500).json({ error: 'An error occurred' });
    }
  }
};

export const burn = async (req: Request, res: Response) => {
  const { tokenId, privateKey } = req.body;
  try {
    const receipt = await burnNFT(tokenId, privateKey);
    res.status(200).json(jsonBigIntStringify(receipt));
  } catch (error) {
    if (error instanceof Error) {
      res.status(500).json({ error: error.message });
    } else {
      res.status(500).json({ error: 'An error occurred' });
    }
  }
};

export const updateMetadata = async (req: Request, res: Response) => {
  const { tokenId } = req.params;
  try {
    await updateNFTMetadata(Number(tokenId));
    res.status(200).json({ message: 'Metadata updated' });
  } catch (error) {
    if (error instanceof Error) {
      res.status(500).json({ error: error.message });
    } else {
      res.status(500).json({ error: 'An error occurred' });
    }
  }
};
