# 📜 LogControllerAPI

API para gerenciamento e transformação de logs.

---

## 🚀 Tecnologias Utilizadas

- ⚙️ **.NET Core 2.1**  
  Framework principal para o desenvolvimento da aplicação.

- 🗄️ **Entity Framework Core**  
  ORM utilizado para gerenciar o banco de dados relacional (SQL Server).

- 🛢️ **SQL Server**  
  Banco de dados utilizado para persistência de logs.

- 🧪 **xUnit**  
  Framework para criação de testes unitários e de integração.

---

## 📖 Exemplos de Uso dos Endpoints

### 1⃣ Adicionar um Log
**Endpoint:** `POST /api/logs`

**Request Body (JSON):**
```json
{
  "originalLog": "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2"
}
```

### 2⃣ Buscar todos os Logs transformado e original
**Endpoint:** `GET /api/logs`

**Response(200):**
```json
[
{
    "originalLog": "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2",
    "transformedLog": "\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT"
  },
  {
    "originalLog": "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2",
    "transformedLog": "\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT"
  },
]
```

### 3⃣ Buscar Log transformado por ID
**Endpoint:** `GET /api/logs/transform/${id}`
**parameters:** `id *integer($int32) required`
                `saveToFile boolean default = false`

**Response(200): save file = false**
```json
{
  "transformedLog": "#Version: 1.0\n#Date: 25/01/2025 18:16:40\n#Fields: provider http-method status-code uri-path time-taken response-size cache-status\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT"
}
```
**Response(200): save file = true**
```json
{
  "filePath": "C:\\Users\\Documents\\LogControllerAPI\\LogTransformer\\LogTransformer.Api\\logs\\log_30_transformed.txt"
}
```
### 4⃣ Buscar Log original por ID
**Endpoint:** `GET /api/Logs/{id}`
**parameters:** `id *integer($int32) required`
            

**Response(200): **
```json
{
  "id": 30,
  "originalLog": "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2",
  "transformedLog": "\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT"
}
```

### 5⃣ Buscar todos Log transformado  por ID
**Endpoint:** `GET /api/Logs/transformed`

        
**Response(200): **
```json
{
  "transformedLogs": "#Version: 1.0\n#Date: 25/01/2025 18:20:27\n#Fields: provider http-method status-code uri-path time-taken response-size cache-status\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT"
}
```
### 6⃣ Salvar Log transformado 
**Endpoint:** `POST /api/Logs/transform`
**parameters:** `saveToFile boolean default = false`
                `Request Body `

**Response(200): request body =
[
  "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2",
  "101|200|MISS|\"POST /myImages HTTP/1.1\"|319.4"
]**

            

**Response(200): **
```json
{
  "transformedLogs": "#Version: 1.0\n#Date: 25/01/2025 18:25:28\n#Fields: provider http-method status-code uri-path time-taken response-size cache-status\n\"MINHA CDN\" GET 200 /robots.txt 1002 312 HIT\n\"MINHA CDN\" POST 200 /myImages 3194 101 MISS"
}
```

**Response(200): request body = "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt"**

        
**Response(200): **
```json
{
  "transformedLogs": "#Version: 1.0\n#Date: 25/01/2025 18:27:08\n#Fields: provider http-method status-code uri-path time-taken response-size cache-status\n\"MINHA CDN\" GET 200 /robots.txt 1002 312 HIT\n\"MINHA CDN\" POST 200 /myImages 3194 101 MISS\n\"MINHA CDN\" GET 404 /not-found 1429 199 MISS\n\"MINHA CDN\" GET 200 /robots.txt 2451 312 REFRESH_HIT"
}
```
