import express from 'express';
import contractRoutes from './routes/contractRoutes';
// // import ipfsRoutes from './routes/ipfsRoutes';
// import nftRoutes from './routes/nftRoutes';

const app = express();

app.use(express.json());

app.use('/contracts', contractRoutes);
// app.use('/ipfs', ipfsRoutes);
// app.use('/nfts', nftRoutes);

export default app;
