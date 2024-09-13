import Web3 from 'web3';
import KYSNFT from '../KYSNFT.json';
import { contractAddress } from '../config';

const web3 = new Web3('http://localhost:7545');

const contract = new web3.eth.Contract<typeof KYSNFT.abi>(KYSNFT.abi, contractAddress);

export const mintNFT = async (to: string, tokenURI: string, privateKey: string) => {
    try {
        const account = web3.eth.accounts.privateKeyToAccount(privateKey);
        const tx = contract.methods.mint(to, tokenURI);

        const gas = await tx.estimateGas({ from: account.address });
        const gasPrice = await web3.eth.getGasPrice();

        const txData = {
            from: account.address,
            to: contractAddress,
            data: tx.encodeABI(),
            gas,
            gasPrice
        };

        const signedTx = await web3.eth.accounts.signTransaction(txData, privateKey);
        const receipt = await web3.eth.sendSignedTransaction(signedTx.rawTransaction || '');
        return receipt;
    } catch (error) {
        console.error('Error minting NFT:', error)
        throw error;
    }
}

export const burnNFT = async (tokenId: number, privateKey: string) => {
    try {
        const account = web3.eth.accounts.privateKeyToAccount(privateKey);
        const tx = contract.methods.burn(tokenId);

        const gas = await tx.estimateGas({ from: account.address });
        const gasPrice = await web3.eth.getGasPrice();

        const txData = {
            from: account.address,
            to: contractAddress,
            data: tx.encodeABI(),
            gas,
            gasPrice
        }

        const signedTx = await web3.eth.accounts.signTransaction(txData, privateKey);
        const receipt = await web3.eth.sendSignedTransaction(signedTx.rawTransaction || '');
        return receipt;
    } catch (error) {
        console.error('Error burning NFT:', error)
        throw error;
    }
}


export const getNFTMetadata = async (tokenId: number): Promise<string> => {
    try {
        const metadataURI: string = await contract.methods.tokenURI(tokenId).call();
        return metadataURI;
    } catch (error) {
        console.error('Error getting NFT metadata:', error)
        throw error;
    }
}