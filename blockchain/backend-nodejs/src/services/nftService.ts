import { saveMintedNFTToDB, saveBurnedNFTToDB, getMetadataFromDB, getNFTsFromDB } from '../db/nftRepository';

export const saveMintedNFT = async (receipt: any, userId: string, address: string, tokenURI: string): Promise<void> => {
    try {
        await saveMintedNFTToDB(receipt, userId, address, tokenURI);
    } catch (error) {
        console.error('Error saving minted NFT:', error)
        throw error;
    }
}

export const saveBurnedNFT = async (tokenId: number): Promise<void> => {
    try {
        await saveBurnedNFTToDB(tokenId);
    } catch (error) {
        console.error('Error saving burned NFT:', error)
        throw error;
    }
}

export const getNFTMetadata = async (tokenId: number): Promise<string> => {
    try {
        const metadataURI = await getMetadataFromDB(tokenId);
        return metadataURI;
    } catch (error) {
        console.error('Error getting NFT metadata:', error)
        throw error;
    }
}

export const getNFTs = async (address: string): Promise<string[]> => {
    try {
        const nfts = await getNFTsFromDB(address);
        return nfts;
    } catch (error) {
        console.error('Error getting NFTs:', error)
        throw error;
    }
}