import aiohttp
import asyncio
import random

# Hedef URL
url = "https://stonks-7s4w.onrender.com/"

# İstek sayısı ve eşzamanlı bağlantı sayısı
total_requests = 5000   # Toplam istek sayısı
concurrent_requests = 1000  # Aynı anda kaç istek gidecek?

# Farklı User-Agent listesi
user_agents = [
    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
    "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Firefox/89.0",
    "Mozilla/5.0 (iPhone; CPU iPhone OS 14_2 like Mac OS X) AppleWebKit/537.36 (KHTML, like Gecko) Safari/537.36",
]

# Kalıcı bağlantılar
sessions = []
async def create_sessions():
    global sessions
    for _ in range(concurrent_requests):  # 1000 kalıcı bağlantı aç
        session = aiohttp.ClientSession()
        sessions.append(session)

async def send_request(session):
    headers = {"User-Agent": random.choice(user_agents)}
    try:
        async with session.get(url, headers=headers) as response:
            print(f"İstek gönderildi! Durum kodu: {response.status}")
    except Exception as e:
        print(f"Hata: {e}")

async def stress_test():
    await create_sessions()
    tasks = [send_request(random.choice(sessions)) for _ in range(total_requests)]
    await asyncio.gather(*tasks)

# Çalıştır
asyncio.run(stress_test())

print("Test tamamlandı!")