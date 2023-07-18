<template>
  <Chat :list-user-active="userTrackerStore.listUserTracker" :messages="messages" @on-send-message="sendMessage" @on-selected-user="onSelectUser"/>
</template>
<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { createSignalRConnection } from '../plugins/connection';
import { useUserTracker } from '../store/userChat';
import Chat from './Chat.vue';

  defineProps<{ msg: string }>()
  const userTrackerStore = useUserTracker();
  const connection = ref<any>(null);
  const messages = ref<any>([])
  const userSelectedToSend = ref<any>({});
  const onSelectUser = (user: any) => {
    userSelectedToSend.value = user;
  }
  onMounted(async () => {
    connection.value = await createSignalRConnection();
    connection.value.on('ReceiveMessage', (user: string, message: string) => {
      messages.value.push({ id: connection.value?.connectionId , user, message, isSelf: (getCookie('email') == user) });
    });
  })
  const sendMessage = async (username: string, message: string) => {
    try {
        await connection.value.invoke("SendMessage", username, message);
    } catch (err) {
        console.error(err);
    }
  }
  const getCookie = (name: string) => {
    const value = `; ${document.cookie}`;
    const parts: any = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
  }
</script>
<style scoped>
.read-the-docs {
  color: #888;
}
</style>
