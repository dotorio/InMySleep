<script setup>
import { ref, onBeforeMount } from "vue";
import CollectionNft from "@/components/collection/CollectionNft.vue";
import { myNFTs } from "@/api/nft";
import { getEquippedSkin, getSkinList } from "@/api/skin";
import { useUserStore } from "@/stores/user";
import { useSkinStore } from "@/stores/skin";
import { useNftStore } from "@/stores/nft";

const uStore = useUserStore();
const sStore = useSkinStore();
const nStore = useNftStore();

// const choice = ref("bear");
// const equippedBear = ref(0);
// const equippedRabbit = ref(0);
const defaultUrl = "ipfs://QmPSSpmQgaHKdiuYnNU8oohNARSLpWpwaaZ4kGKKxSuut6";
const bear = ref({
  nft: [
    {
      imageUrl: defaultUrl,
    },
    {
      imageUrl: defaultUrl,
    },
    {
      imageUrl: defaultUrl,
    },
    {
      imageUrl: defaultUrl,
    },
    {
      imageUrl: defaultUrl,
    },
  ]
});
const rabbit = ref({
  nft: [
    {
      imageUrl: defaultUrl,
    },
    {
      imageUrl: defaultUrl,
    },
    {
      imageUrl: defaultUrl,
    },
    {
      imageUrl: defaultUrl,
    },
    {
      imageUrl: defaultUrl,
    },
  ]
});
const nftData = ref([
  {
    id: 0,
    description: "",
    attributes: {},
    imageUrl: "",
    metadataUri: "",
  }
]);

onBeforeMount(async () => {
  try {
    const response = await getEquippedSkin(uStore.user.data.userId);
    console.log(response);
    sStore.userSkin.selectedBearColor = sStore.userSkin.bearColor = response.data[0].attributes.color;
    sStore.userSkin.selectedRabbitColor = sStore.userSkin.rabbitColor = response.data[1].attributes.color;
    // equippedBear.value = response.data[0].attributes.color;
    // equippedRabbit.value = response.data[1].attributes.color;

    // if (sStore.userSkin.choice === "bear") {
    //   bear.value.nft[equippedBear.value] = response.data[0];
    // } else {
    //   rabbit.value.nft[equippedRabbit.value] = response.data[1];
    // }
  } catch (error) {
    console.error(error);
  }

  try {
    const response = await getSkinList(uStore.user.data.userId);
    nftData.value = response.data;
    nftData.value = prepareNftData(response.data);
    filterNftData(nftData);
    sStore.userBearSkin = bear.value.nft;
    sStore.userRabbitSkin = rabbit.value.nft;
  } catch (error) {
    console.error(error);
  }

  if (!uStore.user.data.metadataToken) {
    return;
  }

  try {
    console.log(uStore.user.data.address);
    const response = await myNFTs(uStore.user.data.address, uStore.user.data.metadataToken);
    nStore.userNft = response.data;
    // if (nftData.value.length > 1) {
    //   nftData.value = prepareNftData(nftData);
    //   console.log(nftData.value);
    //   filterNftData(nftData);
    //   console.log(bear.value.nft);
    //   console.log(rabbit.value.nft);
    // }
  } catch (error) {
    console.error(error);
  }
  // try {
  //   const response = await myNFTs(uStore.user.wallet_address, uStore.user.token);
  //   nftData.value = response.data;
  //   console.log(nftData.value);
  //   if (nftData.value.length > 1) {
  //     nftData.value = prepareNftData(nftData);
  //     console.log(nftData.value);
  //     filterNftData(nftData);
  //     console.log(bear.value.nft);
  //     console.log(rabbit.value.nft);
  //   }
  // } catch (error) {
  //   console.error(error);
  // }
})

// const bear = {
//   nft: ["bear1", "bear2", "QmPSSpmQgaHKdiuYnNU8oohNARSLpWpwaaZ4kGKKxSuut6", "bear3", "QmPSSpmQgaHKdiuYnNU8oohNARSLpWpwaaZ4kGKKxSuut6"],
// };
// const rabbit = {
//   nft: ["rabbit1", "QmPSSpmQgaHKdiuYnNU8oohNARSLpWpwaaZ4kGKKxSuut6", "QmPSSpmQgaHKdiuYnNU8oohNARSLpWpwaaZ4kGKKxSuut6", "rabbit2", "rabbit3"],
// };

function choiceCharacter(character) {
  sStore.userSkin.choice = character;
  choice.value = character;
}

function prepareNftData(responseData) {
  return responseData.map((nft) => {
    return {
      id: nft.id,
      description: nft.description,
      attributes: nft.attributes,
      imageUrl: nft.image_url,
      metadataUri: nft.metadata_uri,
    };
  });
}

function filterNftData(nftData) {
  nftData.value.forEach((nft) => {
    if (nft.attributes.character.toLowerCase() === "bear") {
      bear.value.nft[nft.attributes.color] = nft;
    } else {
      rabbit.value.nft[nft.attributes.color] = nft;
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
    <CollectionNft :nft-data="sStore.userSkin.choice === 'bear' ? bear : rabbit" />
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
