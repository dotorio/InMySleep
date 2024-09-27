import { expressAxios } from "@/utils/request";

const axios = expressAxios();
const baseURL = "contracts";

export function postMint(userId, address, tokenUri) {
  return axios({
    url: `${baseURL}/mint`,
    method: "post",
    data: { userId, address, tokenUri },
  });
}

export function deleteBurn(address, token) {
  return axios({
    url: `${baseURL}/burn`,
    method: "delete",
    data: { address: address, token: token },
  });
}
