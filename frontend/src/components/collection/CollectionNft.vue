<script setup>
defineProps({
  nftData: Object,
});
</script>

<template>
  <div class="nft-con">
    {{ nftData.nft }}
  </div>
</template>

<style scoped>
.nft-con {
  width: 100%;
  height: 80%;
  background-color: blue;
}
</style>
