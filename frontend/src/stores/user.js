import { ref, computed } from "vue";
import { defineStore } from "pinia";

export const useUserStore = defineStore("user", () => {
  const userInfo = ref({
    nickname: "김종덕 만렙",
    nft: "rabbit1",
  });

  function changeNft(nft) {
    userInfo.value.nft = nft;
    console.log(userInfo.value);
  }
  return { userInfo, changeNft };
});
