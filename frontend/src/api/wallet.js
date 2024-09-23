import { expressAxios } from "@/utils/request";

const axios = expressAxios();
const baseURL = "wallet";

export function walletAuth(address, signature, message, username) {
  return axios({
    url: `${baseURL}/auth`,
    method: "post",
    data: {
      address: address,
      signature: signature,
      message: message,
      username: username,
    },
  });
}
