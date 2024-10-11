<script setup>
import { ref } from 'vue';
import { useNftStore } from '@/stores/nft';

const nStore = useNftStore();
const copied = ref(false);

const urlParams = new URLSearchParams(window.location.search);
const metadataId = urlParams.get('metadataId');

const currentNft = nStore.userNft.find(nft => nft.id == metadataId);

function polyscanLink(hash) {
    return `https://polygonscan.com/tx/${hash}`;
}

function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(() => {
        copied.value = true;
        setTimeout(() => {
            copied.value = false;
        }, 1500)
    }).catch(err => {
        console.error('Failed to copy: ', err);
    });
}
</script>

<template>
    <div class="nft-container">
        <h1>NFT</h1>
        <div v-if="currentNft" class="nft-card">
            <div class="nft-details">
                <h2>{{ currentNft.token_id }}</h2>
                <a :href="polyscanLink(currentNft.transaction_hash)" target="_blank">
                    {{ currentNft.transaction_hash.slice(0, 4) }}
                </a>
                <div class="contract-address">
                    <span>Contract:</span>
                    <span @click="copyToClipboard(currentNft.contract_address)" class="copy-address">
                        {{ currentNft.contract_address }}
                    </span>
                    <br />
                </div>
                <span v-if="copied" class="copied-text">Copied!</span>
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

.contract-address {
    margin-top: 10px;
    font-size: 1.1em;
    color: #666;
}

.contract-address span {
    font-weight: bold;
    color: #333;
    cursor: pointer;
    word-wrap: break-word;
    word-break: break-all;
}

.copy-address:hover {
    text-decoration: underline;
    color: #3498db;
}

.copied-text {
    font-size: 0.9em;
    color: green;
    margin-left: 10px;
}

h1 {
    font-size: 2.5em;
    color: #000;
    margin-bottom: 10px;
}
</style>