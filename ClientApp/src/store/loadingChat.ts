import { defineStore } from "pinia";
import { ref } from "vue";

export const useLoadingChat = defineStore('loadingChat',() => {
    const loading = ref(false);
    
    return {
        loading
    }
})