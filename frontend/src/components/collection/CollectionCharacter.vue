<script setup>
import { ref, onBeforeMount } from "vue";
import CollectionNft from "@/components/collection/CollectionNft.vue";
import { myNFTs } from "@/api/nft";
import { useUserStore } from "@/stores/user";

const nftData = ref([
  {
    description: "",
    type: "",
    attributes: {},
    imageUrl: "",
    metadataUri: "",
  }
]);
const uStore = useUserStore();

onBeforeMount(async () => {
  try {
    const response = await myNFTs(uStore.user.wallet_address, uStore.user.token);
    nftData.value = response.data;
    console.log(nftData.value);
    nftData.value = prepareNftData(nftData);
    console.log(nftData.value);
    filterNftData(nftData);
    console.log(bear.value.nft);
    console.log(rabbit.value.nft);
  } catch (error) {
    console.error(error);
  }
});

const choice = ref("bear");
const bear = ref({
  nft: []
});
const rabbit = ref({
  nft: []
});
// const bear = {
//   nft: ["bear1", "bear2", "lock-nft", "bear3", "lock-nft"],
// };
// const rabbit = {
//   nft: ["rabbit1", "lock-nft", "lock-nft", "rabbit2", "rabbit3"],
// };

function choiceCharacter(character) {
  choice.value = character;
}

function prepareNftData(nftData) {
  return nftData.value.nfts.map((nft) => {
    return {
      description: nft.description,
      type: nft.attributes.type,
      attributes: nft.attributes,
      imageUrl: nft.image_url,
      metadataUri: nft.metadata_uri,
    };
  });
}

function filterNftData(nftData) {
  nftData.value.forEach((nft) => {
    if (nft.type.toLowerCase() === "bear") {
      bear.value.nft.push(nft);
    } else if (nft.type.toLowerCase() === "rabbit") {
      rabbit.value.nft.push(nft);
    }
  });
}
</script>

<template>
  <div class="character box-col">
    <div class="choice flex-align">
      <div class="bear box-md" @click="choiceCharacter('bear')">
        <img src="/src/assets/collection/bear.svg" alt="곰" :class="choice === 'bear' ? 'active' : ''" />
      </div>
      <div class="rabbit box-md" @click="choiceCharacter('rabbit')">
        <img src="/src/assets/collection/rabbit.svg" alt="토끼" :class="choice === 'rabbit' ? 'active' : ''" />
      </div>
    </div>
    <CollectionNft :nft-data="choice === 'bear' ? bear : rabbit" />
  </div>
</template>

<style scoped>
.character {
  width: 70%;
  height: 100%;

  justify-content: space-around;
  align-items: center;
  /* background-color: blue; */
}

.choice {
  width: 100%;
  height: 10%;
  justify-content: space-around;
}

.bear img,
.rabbit img {
  width: 80%;
  opacity: 0.5;
  border: 3px solid rgb(22, 22, 115);
  border-radius: 20px;
  cursor: pointer;
  transition: all 0.3s ease-in-out;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.12), 0 2px 6px rgba(0, 0, 0, 0.24);
}

.bear img:hover,
.rabbit img:hover {
  scale: 1.1;
  opacity: 1;
}

.active {
  opacity: 1 !important;
}
</style>
