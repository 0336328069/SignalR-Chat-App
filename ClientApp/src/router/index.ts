import { createWebHistory, createRouter } from "vue-router";
const Error404 = () => import("../views/error/404.vue" as string);
const Error403 = () => import("../views/error/403.vue" as string);
const HomePage = () => import("../views/index.vue" as string);
const LoginPage = () => import("../views/login/index.vue" as string);
const routes = [
  {
    path: "/",
    name: "home-page",
    component: HomePage,
    meta: { requiresAuth: true }
  },
  {
    path: "/login",
    name: "login-page",
    component: LoginPage
  },
  {
    path: "/404",
    name: "404",
    component: Error404,
  },

  {
    path: "/403",
    name: "403",
    component: Error403,
  },
  {
    path: "/:pathMatch(.*)*",
    component: Error404,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  console.log(from);
  const isAuthenticated = getCookie('token');
  if (to.matched.some(record => record.meta.requiresAuth)) {
    if (!isAuthenticated) {
      next('/login');
    } else {
      next();
    }
  } else {
    next();
  }
});

function getCookie(name: string) {
  const value = `; ${document.cookie}`;
  const parts: any = value.split(`; ${name}=`);
  if (parts.length === 2) return parts.pop().split(';').shift();
}

export default router;
