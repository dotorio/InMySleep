import Web3 from 'web3';

const web3 = new Web3('http://localhost:8545');
const contractABI: any[] = []
const contractAddress = '';

const contract = new web3.eth.Contract(contractABI, contractAddress);

async function getNFETMetadata(tokenId: number) {
    const metadataURI = await contract.methods.tokenURI(tokenId).call();
    console.log(metadataURI);
}

export default getNFETMetadata;