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

function getNftSrc(character, color) {
  // return new URL(`/src/assets/collection/nft/${nft}.svg`, import.meta.url).href;
  let hash = "";
  if (sStore.userBearSkin.length === 0 || sStore.userRabbitSkin === 0) {
    return "";
  }
  if (character === "bear") {
    hash = sStore.userBearSkin.filter((skin) => skin.attributes && skin.attributes.color == color)[0].imageUrl.split("ipfs://")[1];
  } else if (character === "rabbit") {
    hash = sStore.userRabbitSkin.filter((skin) => skin.attributes && skin.attributes.color == color)[0].imageUrl.split("ipfs://")[1];
  }
  return new URL(`${VITE_VUE_IPFS_URL}${hash}`, import.meta.url).href;
}

async function equipSkin() {
  let choice = "";
  let selectedColor = "";
  if (sStore.userSkin.choice === "bear") {
    choice = "bear";
    selectedColor = sStore.userSkin.selectedBearColor;
  } else if (sStore.userSkin.choice === "rabbit") {
    choice = "rabbit";
    selectedColor = sStore.userSkin.selectedRabbitColor;
  }
  try {
    const response = await putEquipSkin(uStore.user.userId, choice, selectedColor);
    console.log(response);
    if (response.status === 200) {
      if (choice === "bear") {
        sStore.userSkin.bearColor = sStore.userSkin.selectedBearColor;
      } else if (choice === "rabbit") {
        sStore.userSkin.rabbitColor = sStore.userSkin.selectedRabbitColor;
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
    tokenURI = sStore.userBearSkin.filter((skin) => skin.attributes && skin.attributes.color == sStore.userSkin.selectedBearColor)[0].metadataUri;
  } else if (sStore.userSkin.choice === "rabbit") {
    tokenURI = sStore.userRabbitSkin.filter((skin) => skin.attributes && skin.attributes.color == sStore.userSkin.selectedRabbitColor)[0].metadataUri;
  }
  try {
    const response = await postMint(uStore.user.data.userId, uStore.user.data.address, tokenURI);
    console.log(response);
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
    return sStore.userSkin.selectedBearColor !== sStore.userSkin.bearColor;
  } else if (sStore.userSkin.choice === "rabbit") {
    return sStore.userSkin.selectedRabbitColor !== sStore.userSkin.rabbitColor;
  }
}

function hasNFTCheck() {
  if (sStore.userSkin.choice === "bear") {
    return nStore.userNft.some((nft) => nft.attributes.character === 'bear' && nft.attributes.color == sStore.userSkin.selectedBearColor)
  } else if (sStore.userSkin.choice === "rabbit") {
    return nStore.userNft.some((nft) => nft.attributes.character === 'rabbit' && nft.attributes.color == sStore.userSkin.selectedRabbitColor)
  }
}
</script>

<template>
  <div class="user-con box-col">
    <div class="nickname bit-t">{{ sStore.userSkin.nickname }}</div>
    <img v-if="sStore.userSkin.choice === 'bear'" :src="getNftSrc('bear', sStore.userSkin.selectedBearColor)" alt="곰" />
    <img v-else-if="sStore.userSkin.choice === 'rabbit'" :src="getNftSrc('rabbit', sStore.userSkin.selectedRabbitColor)"
      alt="토끼" />
    <div class="skin-feat">
      <div class="equip box-md">
        <button v-if="equipSkinCheck()" alt="equip" @click="equipSkin()">착용하기</button>
        <button class="disable" v-else alt="equie_diable">착용하기</button>
      </div>
      <div class="equip box-md">
        <button v-if="uStore.user.data.token !== '' && !hasNFTCheck()" alt="gen_nft" @click="mint()">NFT 발행</button>
        <button class="disable" v-else alt="gen_nft_disable">NFT 발행</button>
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
  height: 70%;
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
