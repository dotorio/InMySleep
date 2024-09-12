import { create } from 'ipfs-http-client'

const ipfs = create({ url: 'http://localhost:5001'})

async function uploadFile(file: Buffer) {
    const result = await ipfs.add(file);
    console.log(result.path);
}

export default uploadFile;