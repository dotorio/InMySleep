<script setup>
import { useUserStore } from "@/stores/user";
import { useSkinStore } from "@/stores/skin";
import { useNftStore } from "@/stores/nft";
import { putEquipSkin } from "@/api/skin";
import { postMint } from "@/api/contract";
const { VITE_VUE_IPFS_URL } = import.meta.env;

const uStore = useUserStore();
const sStore = useSkinStore();
const nStore = useNftStore();

function getSkinSrc(character, metadata) {
  let url = "";
  if (sStore.userBearSkin.length === 0 || sStore.userRabbitSkin.length === 0) {
    return "";
  }
  if (character === "bear") {
    sStore.userBearSkin.forEach((skin) => {
      if (skin.id == metadata) {
        url = skin.imageUrl;
      }
    });
    // let filter = sStore.userBearSkin.filter((skin) => skin.id && skin.id == metadata)[0];
    // console.log(filter)
    // if (!filter) {
    //   return "";
    // }
    // url = filter.imageUrl;
  } else if (character === "rabbit") {
    let filter = sStore.userRabbitSkin.filter((skin) => skin.id && skin.id === metadata)[0];
    // if (!filter) {
    //   return "";
    // }
    url = filter.imageUrl;
  }
  return new URL(`${url}`, import.meta.url).href;
}

async function equipSkin() {
  let choice = "";
  let selectedMetadata = "";

  if (sStore.userSkin.choice === "bear") {
    choice = "bear";
    selectedMetadata = sStore.userSkin.selectedBearMetadata;
  } else if (sStore.userSkin.choice === "rabbit") {
    choice = "rabbit";
    selectedMetadata = sStore.userSkin.selectedRabbitMetadata;
  }
  try {
    const response = await putEquipSkin(uStore.user.data.userId, choice, selectedMetadata);
    if (response.status === 200) {
      if (choice === "bear") {
        sStore.userSkin.bearMetadata = sStore.userSkin.selectedBearMetadata;
      } else if (choice === "rabbit") {
        sStore.userSkin.rabbitMetadata = sStore.userSkin.selectedRabbitMetadata;
      }
      Swal.fire({
        icon: "success",
        title: "성공",
        text: "스킨이 변경되었습니다",
      });
    }
  } catch (error) {
    Swal.fire({
      icon: "error",
      title: "실패",
      text: "스킨 변경에 실패했습니다",
    })
    console.error(error);
  }
}

async function mint() {
  let tokenURI = "";
  if (sStore.userSkin.choice === "bear") {
    tokenURI = sStore.userBearSkin.filter((skin) => skin.attributes && skin.id == sStore.userSkin.selectedBearMetadata)[0].metadataUri;
  } else if (sStore.userSkin.choice === "rabbit") {
    tokenURI = sStore.userRabbitSkin.filter((skin) => skin.attributes && skin.id == sStore.userSkin.selectedRabbitMetadata)[0].metadataUri;
  }
  try {
    const response = await postMint(uStore.user.data.userId, uStore.user.data.address, tokenURI);
    if (response.status === 200) {
      Swal.fire({
        icon: "success",
        title: "성공",
        text: "NFT가 발행되었습니다",
      });
    }
  } catch (error) {
    Swal.fire({
      icon: "error",
      title: "실패",
      text: "NFT 발행에 실패했습니다",
    });
    console.error(error);
  }
}

function equipSkinCheck() {
  if (sStore.userSkin.choice === "bear") {
    return sStore.userSkin.selectedBearMetadata !== sStore.userSkin.bearMetadata;
  } else if (sStore.userSkin.choice === "rabbit") {
    return sStore.userSkin.selectedRabbitMetadata !== sStore.userSkin.rabbitMetadata;
  }
}

function hasNFTCheck() {
  if (!uStore.user.data.metamaskToken || nStore.userNft.length === 0) {
    return
  }
  if (sStore.userSkin.choice === "bear") {
    return nStore.userNft.some((nft) => nft.attributes.character === 'bear' && nft.id == sStore.userSkin.selectedBearMetadata)
  } else if (sStore.userSkin.choice === "rabbit") {
    return nStore.userNft.some((nft) => nft.attributes.character === 'rabbit' && nft.id == sStore.userSkin.selectedRabbitMetadata)
  }
}
</script>

<template>
  <div class="user-con box-col">
    <div class="nickname bit-t">{{ uStore.user.data.username }}</div>
    <img v-if="sStore.userSkin.choice === 'bear'" :src="getSkinSrc('bear', sStore.userSkin.selectedBearMetadata)"
      alt="곰" />
    <img v-else-if="sStore.userSkin.choice === 'rabbit'"
      :src="getSkinSrc('rabbit', sStore.userSkin.selectedRabbitMetadata)" alt="토끼" />
    <div class="skin-feat">
      <div class="equip box-md">
        <button v-if="equipSkinCheck()" alt="equip" @click="equipSkin()">착용하기</button>
        <button class="disable" v-else alt="equie_diable">착용하기</button>
      </div>
      <div class="equip box-md">
        <button v-if="uStore.user.data.metamaskToken && !hasNFTCheck()" alt="gen_nft" @click="mint()">NFT
          발행</button>
        <button class="disable" v-else alt="gen_nft_disable">NFT 보유</button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.user-con {
  width: 30%;
  height: 100%;

  border-radius: 20px;
  background-color: white;
}

.nickname {
  font-size: 30px;
  margin: 20px 30px;
}

img {
  height: 68%;
  margin-left: 5%;
  margin-top: 1%;
  margin-bottom: 1%;
  width: 90%;
}

.equip {
  width: 80%;
  height: 100%;
}

.equip>button {
  width: 80%;
  height: 100%;
  border: 3px solid rgb(22, 22, 115);
  font-size: 20px;
  font-weight: bold;
  color: cadetblue;
  font-family: "Galmuri11";
  background-color: azure;
  border-radius: 40px;
  cursor: pointer;
  transition: all 0.3s ease-in-out;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.12), 0 2px 6px rgba(0, 0, 0, 0.24);
}

.equip>button:hover {
  transform: scale(1.1);
  /* scale: 1.1; */
  opacity: 1;
}

.disable {
  transform: scale(1) !important;
  cursor: not-allowed !important;
  filter: brightness(0.7);
  opacity: 0.7 !important;
}

.skin-feat {
  display: flex;
  justify-content: space-around;
  margin-top: 20px;
}
</style>
