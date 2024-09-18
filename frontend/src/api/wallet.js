import { localAxios } from "@/utils/request";

const axios = localAxios();
const baseURL = "wallet";

export function walletAuth(address, signature) {
  return axios({
    url: `${baseURL}/auth`,
    method: "post",
    data: { address, signature },
  });
}
