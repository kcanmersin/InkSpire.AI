import * as signalR from "@microsoft/signalr";

const gameHubConnection = new signalR.HubConnectionBuilder()
  .withUrl("http://localhost:5256/gameHub")
  .configureLogging(signalR.LogLevel.Information)
  .withAutomaticReconnect()
  .build();

gameHubConnection
  .start()
  .then(() => console.log("[SignalR] Connected to gameHub"))
  .catch(err => console.error("[SignalR] Connection Error:", err));

export default gameHubConnection;
