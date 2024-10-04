import { createRouter, createWebHistory } from "vue-router";
import HomeView from "@/views/HomeView.vue";
import CollectionView from "@/views/CollectionView.vue";
import SignUpView from "@/views/SignUpView.vue";
import LoginView from "@/views/LoginView.vue";
import { useUserStore } from "@/stores/user";
import { storeToRefs } from "pinia";
import ChangePasswordView from "@/views/ChangePasswordView.vue";

const onlyAuthUser = (to, from, next) => {
  const ustore = useUserStore();
  const { user } = storeToRefs(ustore);
  if (user.value) {
    next();
  } else {
    Swal.fire({
      icon: "error",
      title: "로그인이 필요한 기능입니다.",
    }).then(() => {
      next({ name: "login" });
    });
  }
};

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      component: HomeView,
    },
    {
      path: "/collection",
      name: "collection",
      component: CollectionView,
      beforeEnter: onlyAuthUser,
    },
    {
      path: "/signup",
      name: "signup",
      component: SignUpView,
    },
    {
      path: "/login",
      name: "login",
      component: LoginView,
    },
    {
      path: "/chgpwd",
      name: "change-password",
      component: ChangePasswordView,
    },
  ],
});

export default router;
