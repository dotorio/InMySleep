import { ref } from "vue";
import { defineStore } from "pinia";

export const useUserStore = defineStore(
  "user",
  () => {
    const user = ref("");

    const userInfo = ref({
      nickname: "김종덕 만렙",
      nft: "rabbit1",
    });

    function changeNft(nft) {
      if (nft === "lock-nft") {
        Swal.fire({
          icon: "error",
          title: "NFT를 가지고 있지 않습니다",
          text: "스토리를 클리어하면서 NFT를 수집해보세요!",
        });
        return;
      }
      userInfo.value.nft = nft;
      console.log(userInfo.value);
    }

    function userLogin(data) {
      user.value = data;
    }

    function userLogout() {
      user.value = "";
    }

    return { user, userInfo, changeNft, userLogin, userLogout };
  },
  { persist: true }
);
