# Server-Sent Events Demo with .NET 10

A demonstration project showcasing real-time server-to-client communication using **Server-Sent Events (SSE)** in ASP.NET Core with .NET 10's native support.

## 🎯 Overview

This project demonstrates the implementation of Server-Sent Events, a technology that enables servers to push real-time updates to clients over HTTP. Unlike WebSockets, SSE provides a unidirectional communication channel from server to client, making it ideal for scenarios like live notifications, real-time dashboards, and continuous data feeds.

## ✨ Features

- **Native .NET 10 SSE Support**: Utilizes the built-in `System.Net.ServerSentEvents` namespace
- **Real-Time Weather Updates**: Streams weather forecast data every second
- **Event ID Tracking**: Implements event identification for connection recovery
- **Reconnection Handling**: Supports automatic reconnection with `Last-Event-ID` header
- **OpenAPI Integration**: Includes API documentation support
- **Minimal API Design**: Clean and modern ASP.NET Core Minimal API pattern

## 🛠️ Technologies

- **.NET 10** - Latest .NET framework
- **ASP.NET Core** - Web framework
- **System.Net.ServerSentEvents** - Native SSE support
- **OpenAPI** - API documentation

## 📋 Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- A code editor (Visual Studio 2022, VS Code, or JetBrains Rider)
- cURL or a browser with SSE support for testing

## 🚀 Getting Started

### Installation

1. Clone the repository:
```bash
git clone https://github.com/iamhitya/server-sent-events.git
cd server-sent-events
```

2. Navigate to the WebApi project:
```bash
cd WebApi
```

3. Restore dependencies:
```bash
dotnet restore
```

4. Run the application:
```bash
dotnet run
```

The API will start on `https://localhost:5001` (HTTPS) and `http://localhost:5000` (HTTP).

## 📡 Usage

### Testing with cURL

```bash
curl -N http://localhost:5285/weatherforecast
```

Expected output (streaming):
```
event: weatherForecast
id: 2026-01-02T10:30:00.0000000Z
retry: 500
data: {"Date":"2026-01-02","TemperatureC":25,"Summary":"Warm","TemperatureF":77}

event: weatherForecast
id: 2026-01-02T10:30:01.0000000Z
retry: 500
data: {"Date":"2026-01-02","TemperatureC":18,"Summary":"Cool","TemperatureF":64}
```

### Testing with JavaScript

```html
<!DOCTYPE html>
<html>
<head>
    <title>SSE Weather Demo</title>
</head>
<body>
    <h1>Real-Time Weather Updates</h1>
    <div id="weather"></div>

    <script>
        const eventSource = new EventSource('http://localhost:5285/weatherforecast');
        
        eventSource.addEventListener('weatherForecast', (event) => {
            const data = JSON.parse(event.data);
            document.getElementById('weather').innerHTML = `
                <p>Date: ${data.Date}</p>
                <p>Temperature: ${data.TemperatureC}°C / ${data.TemperatureF}°F</p>
                <p>Summary: ${data.Summary}</p>
                <p>Event ID: ${event.lastEventId}</p>
                <hr>
            `;
        });

        eventSource.onerror = (error) => {
            console.error('SSE Error:', error);
        };
    </script>
</body>
</html>
```

## 🔑 Key Concepts Demonstrated

### 1. Server-Sent Events Protocol
The project implements the SSE protocol with:
- **Event Type**: `weatherForecast` for structured event handling
- **Event ID**: Timestamped unique identifiers for each message
- **Retry Interval**: 500ms reconnection delay

### 2. Reconnection Support
Handles client reconnections using the `Last-Event-ID` header, allowing clients to resume from where they left off.

### 3. Cancellation Token Support
Proper async enumeration with cancellation support for graceful shutdown.

### 4. Type-Safe Event Streaming
Uses `SseItem<T>` for strongly-typed event data serialization.

## 📂 Project Structure

```
server-sent-events/
├── WebApi/
│   ├── Program.cs              # Main application entry point with SSE endpoint
│   └── ServerSentEvents.csproj # Project file
└── README.md                   # This file
```

## 🎓 Learning Points

This project demonstrates:

1. **Modern .NET Features**: Utilizes .NET 10's native SSE support
2. **Async Streams**: Implements `IAsyncEnumerable<T>` for efficient data streaming
3. **Minimal APIs**: Clean, concise API design without controllers
4. **Real-Time Communication**: Server-push architecture for live updates
5. **Connection Resilience**: Event IDs and reconnection handling

## 🔄 When to Use Server-Sent Events

SSE is ideal for:
- ✅ Real-time notifications and alerts
- ✅ Live feeds (news, social media, sports scores)
- ✅ Progress updates for long-running operations
- ✅ Stock tickers and financial data streams
- ✅ IoT sensor data monitoring

Consider WebSockets if you need:
- ❌ Bidirectional communication
- ❌ Binary data transmission
- ❌ Lower latency requirements