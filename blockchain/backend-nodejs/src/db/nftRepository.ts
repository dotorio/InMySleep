
import { connectDB } from './dbConnection';
import { FieldPacket } from 'mysql2';
import web3 from 'web3';

export const saveMintedNFTToDB = async (receipt: any, to: string, tokenURI: string) => {
    try {
      const conn = await connectDB();
      const [metadata_id]: [any[], FieldPacket[]] = await conn.query('SELECT id FROM metadata WHERE metadata_uri = ?', [tokenURI]);
      const token_id = web3.utils.hexToNumber(receipt.logs[1].data);
      const [rows] = await conn.query('INSERT INTO nfts (token_id, contract_address, owner_address, metadata_id) VALUES (?, ?, ?, ?)', [token_id, receipt.transactionHash, to, metadata_id[0].id]);
      console.log('Saved minted NFT to DB:', rows);
    } catch (error) {
      console.error('Error saving minted NFT to DB:', error);
      throw error;
    }
  }
  
  export const saveBurnedNFTToDB = async (tokenId: number) => {
    try {
      const conn = await connectDB();
      const [rows] = await conn.query('DELETE FROM nfts WHERE token_id = ?', [tokenId]);
      console.log('Saved burned NFT to DB:', rows);
    } catch (error) {
      console.error('Error saving burned NFT to DB:', error);
      throw error;
    }
  }
  
  export const getMetadataFromDB = async (tokenId: number): Promise<string> => {
    try {
      const conn = await connectDB();
      const [rows]: [any[], FieldPacket[]] = await conn.query('SELECT metadata_uri FROM metadata WHERE id = (SELECT metadata_id FROM nfts WHERE token_id = ?)', [tokenId]);
      return rows[0].metadata_uri;
    } catch (error) {
      console.error('Error getting metadata from DB:', error);
      throw error;
    }
  }
  
  export const saveMetadataToDB = async (tokenId: number, metadataURI: string) => {
    try {
      const conn = await connectDB();
      const [metadata_id]: [any[], FieldPacket[]] = await conn.query('SELECT id FROM metadata WHERE metadata_uri = ?', [metadataURI]);
      const [rows] = await conn.query('UPDATE nfts SET metadata_id = ? WHERE token_id = ?', [metadata_id[0].id, tokenId]);
      console.log('Saved metadata to DB:', rows);
    } catch (error) {
      console.error('Error saving metadata to DB:', error);
      throw error;
    }
  }