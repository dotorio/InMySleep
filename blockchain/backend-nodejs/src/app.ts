import express from 'express';
import cors from 'cors';
import contractRoutes from './routes/contractRoutes';
// // import ipfsRoutes from './routes/ipfsRoutes';
import nftRoutes from './routes/nftRoutes';
import walletRoutes from './routes/walletRoutes';

const app = express();

app.use(express.json());
const corsOptions = {
    origin: '*',
    optionsSuccessStatus: 200,
};
app.use(cors(corsOptions));

const BASE_URL = '/api/v1';
app.use((req, res, next) => {
    res.on('finish', () => {
        console.log(`${req.method} ${req.originalUrl} ${req.url} ${res.statusCode}`);
    });
    next();
});
app.use(`${BASE_URL}/contracts`, contractRoutes);
// app.use('/ipfs', ipfsRoutes);
app.use(`${BASE_URL}/nfts`, nftRoutes);
app.use(`${BASE_URL}/wallet`, walletRoutes);

app.get('/', (req, res) => {
    res.send('Hello World!');
});

export default app;
