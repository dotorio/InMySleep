<script setup>
import { useNftStore } from '@/stores/nft';

const nStore = useNftStore();

const urlParams = new URLSearchParams(window.location.search);
const metadataId = urlParams.get('metadataId');

const currentNft = nStore.userNft.find(nft => nft.id == metadataId);

function polyscanLink(hash) {
    return `https://polygonscan.com/tx/${hash}`;
}
</script>

<template>
    <div class="nft-container">
        <h1>NFT INFO</h1>
        <div v-if="currentNft" class="nft-card">
            <div class="nft-details">
                <h2>토큰 번호: {{ currentNft.token_id }}</h2>
                <a :href="polyscanLink(currentNft.transaction_hash)" target="_blank">트랜잭션 조회: {{
                    currentNft.transaction_hash.slice(0, 4) }}</a>
            </div>
        </div>
    </div>
</template>

<style scoped>
.nft-container {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
    margin-top: 20px;
    font-family: 'Arial', sans-serif;
}

.nft-card {
    background-color: #f5f5f5;
    border: 3px solid #000;
    border-radius: 10px;
    padding: 20px;
    box-shadow: 5px 5px 15px rgba(0, 0, 0, 0.2);
    text-align: center;
    width: 300px;
    position: relative;
}

.nft-details {
    background-color: #fff;
    padding: 10px;
    border-radius: 5px;
    box-shadow: 3px 3px 10px rgba(0, 0, 0, 0.1);
}

.nft-card h2 {
    font-size: 2em;
    color: #333;
    margin: 10px 0;
}

.nft-card a {
    font-size: 1.2em;
    color: #3498db;
    text-decoration: none;
}

.nft-card a:hover {
    text-decoration: underline;
}

h1 {
    font-size: 2.5em;
    color: #000;
    margin-bottom: 10px;
}
</style>