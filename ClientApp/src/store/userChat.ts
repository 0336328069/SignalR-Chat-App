import { defineStore } from "pinia";
import { ref } from "vue";

export const useUserTracker = defineStore('userTracker',() => {
    const listUserTracker = ref([]);
    
    return {
        listUserTracker
    }
})