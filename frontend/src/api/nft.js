import { expressAxios } from "@/utils/request";

const axios = expressAxios();
const baseURL = "nfts";

export function myNFTs(address, token) {
  return axios({
    url: `${baseURL}/mynfts`,
    method: "get",
    params: { address: address, token: token },
  });
}
