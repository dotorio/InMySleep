import { ref } from "vue";
import { defineStore } from "pinia";
import { useSkinStore } from "./skin";
import { useNftStore } from "./nft";
// import { createPinia, setActivePinia } from 'pinia';

// setActivePinia(createPinia());

export const useUserStore = defineStore(
  "user",
  () => {
    const user = ref("");

    // const userInfo = ref({
    //   nickname: "김종덕 만렙",
    //   choice: "bear",
    //   bearMetadata: "0",
    //   selectedBearMetadata: "0",
    //   rabbitMetadata: "0",
    //   selectedRabbitMetadata: "0",
    // });

    // const userBearSkin = ref([]);
    // const userRabbitSkin = ref([]);

    // function changeNft(nft) {
    //   if (nft === "lock-nft") {
    //     Swal.fire({
    //       icon: "error",
    //       title: "NFT를 가지고 있지 않습니다",
    //       text: "스토리를 클리어하면서 NFT를 수집해보세요!",
    //     });
    //     return;
    //   }
    //   userInfo.value.nft = nft;
    //   console.log(userInfo.value);
    // }

    function userLogin(data) {
      user.value = data;
    }

    function userLogout() {
      user.value = "";

      const sStore = useSkinStore();
      const nStore = useNftStore();

      sStore.userSkin = {};
      sStore.userBearSkin = [];
      sStore.userRabbitSkin = [];
      nStore.userNft = "";
    }

    return {
      user,
      userLogin,
      userLogout,
    };
  },
  { persist: true }
);
