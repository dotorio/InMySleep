<script setup>
import CollectionCharacter from "@/components/collection/CollectionCharacter.vue";
import UserInfo from "@/components/collection/UserInfo.vue";
</script>

<template>
  <div class="collection-con box-md">
    <div class="content">
      <span class="category bit-t">My NFT</span>
      <div class="main flex-align">
        <UserInfo />
        <CollectionCharacter />
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
