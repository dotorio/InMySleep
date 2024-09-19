import { getMetadataFromDB } from '../db/nftRepository';

export const getNFTMetadata = async (tokenId: number): Promise<string> => {
    try {
        const metadataURI = await getMetadataFromDB(tokenId);
        return metadataURI;
    } catch (error) {
        console.error('Error getting NFT metadata:', error)
        throw error;
    }
}

// export const getNFTs = async (address: string): Promise<string[]> => {
//     try {
//         const nfts = await getNFTsFromDB(address);
//         return nfts;
//     } catch (error) {
//         console.error('Error getting NFTs:', error)
//         throw error;
//     }
// }