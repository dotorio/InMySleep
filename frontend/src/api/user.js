import { localAxios } from "@/utils/request";

const axios = localAxios();
const baseURL = "user";

export function signUp(data) {
  return axios({
    url: `auth/signup`,
    method: "post",
    data,
  });
}

export function emailCheck(params) {
  return axios({
    url: `${baseURL}/check-email`,
    method: "get",
    params: params,
  });
}
