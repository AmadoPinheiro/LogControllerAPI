# LogControllerAPI

## 🚀 Tecnologias Utilizadas

- ⚙️ **.NET Core 2.1**  
  Framework principal para desenvolvimento da aplicação.

- 🗄️ **Entity Framework Core**  
  ORM utilizado para gerenciar o banco de dados relacional (SQL Server).

- 🛢️ **SQL Server**  
  Banco de dados para persistência de logs.

- 🧪 **xUnit**  
  Framework para criação de testes unitários e de integração.


Exemplos de Uso dos Endpoints
1. Adicionar um Log
Endpoint: POST /api/logs

Request Body (JSON):
{
  "originalLog": "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2"
}

Response (200):"Log salvo com sucesso!"


2. Transformar um Log (Salvar em Arquivo)
Endpoint: GET /api/logs/transform/{id}?saveToFile=true

Response (200):
{
  "filePath": "/path/to/log_1_transformed.txt"
}

3. Buscar Todos os Logs
Endpoint: GET /api/logs

Response (200):[
  {
    "originalLog": "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2",
    "transformedLog": "\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT"
  }
]

4. Buscar Logs Transformados
Endpoint: GET /api/logs/transformed

Response (200):{
  "transformedLogs": "#Version: 1.0\n#Date: 15/12/2017 23:01:06\n#Fields: provider http-method status-code uri-path time-taken response-size cache-status\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT"
}

5. Buscar um Log por ID
Endpoint: GET /api/logs/{id}

Response (200):
{
  "id": 1,
  "originalLog": "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2",
  "transformedLog": "\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT"
}

6. Endpoint: POST /api/logs/transform

Este endpoint permite transformar uma lista de logs no formato "MINHA CDN" para o formato "Agora". A entrada pode ser uma URL ou uma lista de logs.

Request Body (JSON):

[
  "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2",
  "101|200|MISS|\"POST /myImages HTTP/1.1\"|319.4"
]
Ou apenas uma URL:

"https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt"

Query Parameters:

saveToFile (opcional): Se for true, salva os logs transformados em um arquivo.


Response (200) - Sem salvar em arquivo:

{
  "TransformedLogs": "#Version: 1.0\n#Date: 15/12/2017 23:01:06\n#Fields: provider http-method status-code uri-path time-taken response-size cache-status\n\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT\n\"MINHA CDN\" POST 200 /myImages 319 101 MISS"
}

Response (200) - Com salvar em arquivo:


{
  "FilePath": "/path/to/logs_transformed_20250101.txt"
}
