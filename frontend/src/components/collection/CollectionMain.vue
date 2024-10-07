<script setup>
import { onBeforeMount, ref } from "vue";
import CollectionCharacter from "@/components/collection/CollectionCharacter.vue";
import UserInfo from "@/components/collection/UserInfo.vue";
import { myNFTs } from "@/api/nft";
import { getEquippedSkin, getSkinList } from "@/api/skin";
import { useSkinStore } from "@/stores/skin";
import { useUserStore } from "@/stores/user";

const sStore = useSkinStore();
const uStore = useUserStore();

const defaultUrl = "none";
const bear = ref({
  nft: Array.from({ length: 15 }, () => ({
    imageUrl: defaultUrl,
  })),
});
const rabbit = ref({
  nft: Array.from({ length: 15 }, () => ({
    imageUrl: defaultUrl,
  })),
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
  sStore.userSkin["choice"] = "bear";
  try {
    const response = await getEquippedSkin(uStore.user.data.userId);
    if (response.data[0].attributes.character === "bear") {
      sStore.userSkin.selectedBearMetadata = sStore.userSkin.bearMetadata = response.data[0].id;
      sStore.userSkin.selectedRabbitMetadata = sStore.userSkin.rabbitMetadata = response.data[1].id;
    } else {
      sStore.userSkin.selectedRabbitMetadata = sStore.userSkin.rabbitMetadata = response.data[0].id;
      sStore.userSkin.selectedBearMetadata = sStore.userSkin.bearMetadata = response.data[1].id;
    }
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

  if (!uStore.user.data.metamaskToken) {
    return;
  }

  try {
    const response = await myNFTs(uStore.user.data.address, uStore.user.data.metamaskToken);
    nStore.userNft = response.data;
  } catch (error) {
    Swal.fire({
      icon: "error",
      title: "NFT를 불러오는데 실패했습니다. 지갑 연동 상태를 확인해주세요.",
    });
    uStore.user.data.metamaskToken = "";
    uStore.user.data.address = "";
    console.error(error);
  }
})


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
  let bearIndex = 0
  let rabbitIndex = 0
  nftData.value.forEach((nft) => {
    if (nft.attributes.character.toLowerCase() === "bear") {
      bear.value.nft[bearIndex] = nft;
      bearIndex++;
    } else {
      rabbit.value.nft[rabbitIndex] = nft;
      rabbitIndex++;
    }
  });
}
</script>

<template>
  <div class="collection-con box-md">
    <div class="content">
      <span class="category bit-t">My Skin</span>
      <div class="main flex-align">
        <UserInfo />
        <CollectionCharacter :nft-data="sStore.userSkin.choice === 'bear' ? bear : rabbit" />
      </div>
    </div>
  </div>
</template>

<style scoped>
.collection-con {
  height: 87vh;
  justify-content: center;
  background-color: #d3caf7;
}

.category {
  position: absolute;
  top: -50px;
  font-size: 30px;
}

.content {
  padding-top: 20px;
  width: 80vw;
  height: 70vh;

  position: relative;
  /* background-color: burlywood; */
}

.main {
  width: 100%;
  height: 100%;
}
</style>
