import { localAxios } from "@/utils/request";

const axios = localAxios();
const baseURL = "nfts";

export function myNFTs(address, token) {
  return axios({
    url: `${baseURL}/mynfts`,
    method: "get",
    params: { address: address, token: token },
  });
}
