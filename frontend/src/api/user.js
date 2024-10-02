import { springAxios } from "@/utils/request";

const axios = springAxios();
const baseURL = "user";

export function signUp(data) {
  return axios({
    url: `auth/signup`,
    method: "post",
    data,
  });
}

export function login(data) {
  return axios({
    url: `auth/login`,
    method: "post",
    data,
  });
}

export function logout() {
  return axios({
    url: `auth/logout`,
    method: "post",
  });
}

export function emailCheck(email) {
  return axios({
    url: `${baseURL}/check-email`,
    method: "get",
    params: { email },
  });
}

export function emailAuth(email) {
  return axios({
    url: `auth/email/verification-request`,
    method: "post",
    params: { email },
  });
}

export function emailAuthCheck(email, code) {
  return axios({
    url: `auth/emails/verifications`,
    method: "get",
    params: { email, code },
  });
}

export function usernameCheck(username) {
  return axios({
    url: `${baseURL}/check-username`,
    method: "get",
    params: { username },
  });
}

export function changePassword(data) {
  return axios({
    url: `auth/change-password`,
    method: "post",
    data,
  });
}

// 가입한 유저인지 체크 - 비밀번호 변경 용
export function emailUserAuth(email) {
  return axios({
    url: `user/check-email-user`,
    method: "get",
    params: { email },
  });
}

// 비밀번호 변경용 이메일 인증 코드 전송
export function emailChangePasswordCode(email) {
  return axios({
    url: `auth/email/verification-chgpw-request`,
    method: "post",
    params: { email },
  });
}

// 비밀번호 변경용 이메일 인증 체크
export function emailChangePasswordCodeCheck(email, code) {
  return axios({
    url: `auth/emails/verifications-chgpw`,
    method: "get",
    params: { email, code },
  });
}

// 이메일 인증으로 비밀번호 변경
export function changeEmailPassword(data) {
  return axios({
    url: 'auth/change-email-password',
    method: "patch",
    data,
  });
}