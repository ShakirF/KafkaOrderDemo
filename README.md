# KafkaOrderDemo (.NET 8 + Apache Kafka)

This project demonstrates a real-world usage of **Apache Kafka** with **.NET 8** using two services:

- 📤 `OrderService`: A minimal Web API that publishes order messages to Kafka.
- 📥 `NotificationService`: A background worker service that listens for Kafka messages and processes them (simulating sending notifications).

---

## 🔧 Tech Stack

- [.NET 8](https://dotnet.microsoft.com/)
- [Apache Kafka](https://kafka.apache.org/)
- [Docker Compose](https://docs.docker.com/compose/)
- [Confluent.Kafka NuGet](https://www.nuget.org/packages/Confluent.Kafka)

---

## 📁 Project Structure

```
KafkaOrderDemo/
├── OrderService/             # Kafka producer (Web API)
├── NotificationService/      # Kafka consumer (Background service)
├── docker-compose.yml        # Starts Kafka + Zookeeper
└── README.md                 # This file
```

---

## 🚀 How to Run

### 1. 🐳 Start Kafka with Docker

Make sure Docker Desktop is running. Then in the project root:

```bash
docker compose up -d
```

> This starts Kafka and Zookeeper in the background.

---

### 2. 🧑‍💻 Create Kafka Topic

Open the Kafka container and run:

```bash
docker exec -it <your_kafka_container> bash
kafka-topics --bootstrap-server localhost:9092 --create --topic order-topic --partitions 1 --replication-factor 1
exit
```

Replace `<your_kafka_container>` with the container name shown in `docker ps`.

---

### 3. ▶️ Run OrderService (Producer)

```bash
cd OrderService
dotnet run
```

---

### 4. ▶️ Run NotificationService (Consumer)

In another terminal:

```bash
cd NotificationService
dotnet run
```

---

## 🧪 Test It

Send a POST request to `OrderService`:

```http
POST http://localhost:5079/order
Content-Type: application/json

{
  "id": "11111111-2222-3333-4444-555555555555",
  "customerName": "Alice",
  "product": "Keyboard",
  "quantity": 2
}
```

If everything works:
- You’ll see `✅ Order published to Kafka` in the API response.
- You’ll see `📨 New Order Received...` in the NotificationService logs.

---

## 📚 Notes

- The Kafka topic **`order-topic`** must be created before starting the consumer.
- `localhost:9092` is used as the broker address.
- The `.NET Kafka client` used here is [Confluent.Kafka](https://github.com/confluentinc/confluent-kafka-dotnet).

---

## 📄 License

This project is for learning/demo purposes. You can reuse or extend it freely.

---
