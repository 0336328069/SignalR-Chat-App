import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr'
import { useLoadingChat } from '../store/loadingChat';
import { useUserTracker } from '../store/userChat';

let connection = null as any; 

export async function createSignalRConnection() {
  if (connection) {
    return connection;
  }
  const loadingStore = useLoadingChat();
  const userTrackerStore = useUserTracker();
  try {
    connection = new HubConnectionBuilder()
      .configureLogging(LogLevel.Error)
      .withUrl('https://localhost:7115/chathub', {
        accessTokenFactory: () => getCookie('token')
      })
      .withAutomaticReconnect()
      .build()
    await connection.start();
    connection.on('NewUserLoggedIn',async () => {
      userTrackerStore.listUserTracker = await connection.invoke('GetConnectedUsers');
    })
    loadingStore.loading = true;
    // Thực hiện các xử lý khi kết nối thành công
  } catch (error) {
    console.error('SignalR connection error: ', error)
  }

  return connection
}

const getCookie = (name: string) => {
  const value = `; ${document.cookie}`;
  const parts: any = value.split(`; ${name}=`);
  if (parts.length === 2) return parts.pop().split(';').shift();
}