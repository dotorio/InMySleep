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

export function emailCheck(email) {
  return axios({
    url: `${baseURL}/check-email`,
    method: "get",
    params: { email },
  });
}

export function usernameCheck(username) {
  console.log(11);
  return axios({
    url: `${baseURL}/check-username`,
    method: "get",
    params: { username },
  });
}
