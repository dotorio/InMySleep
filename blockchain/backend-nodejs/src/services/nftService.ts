import { getMetadataFromDB } from '../db';

export const getNFTMetadata = async (tokenId: number): Promise<string> => {
    try {
        const metadataURI = await getMetadataFromDB(tokenId);
        return metadataURI;
    } catch (error) {
        console.error('Error getting NFT metadata:', error)
        throw error;
    }
}