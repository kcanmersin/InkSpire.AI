import * as signalR from "@microsoft/signalr";

const hubConnection = new signalR.HubConnectionBuilder()
  .withUrl("http://localhost:5256/bookHub", {
    withCredentials: false,
  })
  .configureLogging(signalR.LogLevel.Information)
  .withAutomaticReconnect()
  .build();

hubConnection.on("bookcreated", (bookTitle: string) => {
  console.log(`[SignalR] İstemci olay aldı: bookcreated - ${bookTitle}`);
});

hubConnection
  .start()
  .then(() => console.log("[SignalR] Connected to bookHub"))
  .catch(err => console.error("[SignalR] Connection Error:", err));

export default hubConnection;
