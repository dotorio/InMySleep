import axios from "axios";

const { VITE_VUE_SPRING_API_URL, VITE_VUE_EXPRESS_API_URL } = import.meta.env;

function springAxios() {
  const instance = axios.create({
    baseURL: VITE_VUE_SPRING_API_URL + "api/v1/",
  });
  instance.defaults.headers.common["Authorization"] = "";
  instance.defaults.headers.post["Content-Type"] = "application/json";
  instance.defaults.headers.put["Content-Type"] = "application/json";

  return instance;
}

function expressAxios() {
  const instance = axios.create({
    baseURL: VITE_VUE_EXPRESS_API_URL + "api/v1/",
  });
  instance.defaults.headers.common["Authorization"] = "";
  instance.defaults.headers.post["Content-Type"] = "application/json";
  instance.defaults.headers.put["Content-Type"] = "application/json";

  return instance;
}

export { springAxios, expressAxios };
