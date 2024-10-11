import { expressAxios } from "@/utils/request";

const axios = expressAxios();
const baseURL = "skins";

export function getEquippedSkin(userId) {
  return axios({
    url: `${baseURL}/equipped`,
    method: "get",
    params: { userId },
  });
}

export function getSkinList(userId) {
  return axios({
    url: `${baseURL}/list`,
    method: "get",
    params: { userId },
  });
}

export function putEquipSkin(userId, character, metadataId) {
  return axios({
    url: `${baseURL}/equip`,
    method: "put",
    data: { userId, character, metadataId },
  });
}
