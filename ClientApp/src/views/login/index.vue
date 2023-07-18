<template>
    <Form @on-submit="connectSocket" v-model:email="email" v-model:password="password"></Form>
</template>
<script lang="ts" setup name="login-page">
import Form from "../../components/LoginForm.vue";
import axios from 'axios';
import { nextTick,onMounted,ref } from "vue";
import router from "../../router";
const email = ref("");
const password = ref("");

const getCookie = (name: string) => {
  const value = `; ${document.cookie}`;
  const parts: any = value.split(`; ${name}=`);
  if (parts.length === 2) return parts.pop().split(';').shift();
}

const connectSocket = async () => {
  const { data, error } = await axios.post('https://localhost:7115/api/auth/login', {
    username: email.value,
    password: password.value,
  }) as any;
  if(data?.accessToken && !error){
    nextTick(() => {
      document.cookie = `email=${email.value}`;
      document.cookie = `token=${data?.accessToken}`
      router.push({ path : '/' });
    })
  }
}
onMounted(() => {
  if(getCookie('token')){
    router.push('/')
  }
})
</script>