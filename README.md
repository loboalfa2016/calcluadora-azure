# Calculadora Científica & Convertidor (.NET 10)

Aplicación construida en **.NET 10** bajo una arquitectura de microservicios contenerizada con Docker:
- **Backend API**: ASP.NET Core Web API en .NET 10 (`http://localhost:8081`)
- **Frontend MVC**: ASP.NET Core MVC en .NET 10 (`http://localhost:8080`)

---

## 🚀 Inicio Rápido con Docker Compose

Desde la carpeta raíz del proyecto:

```bash
# 1. Construir e iniciar ambos contenedores en segundo plano
docker compose up -d

# 2. Ver el estado de los contenedores
docker compose ps

# 3. Ver logs en tiempo real
docker compose logs -f

# 4. Detener los contenedores
docker compose down
```

---

## 🌐 URLs de Acceso Local

- **Frontend MVC (Interfaz de Usuario):** [http://localhost:8080](http://localhost:8080)
- **Backend API (Healthcheck):** [http://localhost:8081/api/calculatorapi/health](http://localhost:8081/api/calculatorapi/health)
- **Backend OpenAPI:** [http://localhost:8081/openapi/v1.json](http://localhost:8081/openapi/v1.json)

---

## 🧪 Pruebas de API vía cURL

### 1. Cálculo Científico (seno de 90°):
```bash
curl -X POST http://localhost:8081/api/calculatorapi/calculate \
  -H "Content-Type: application/json" \
  -d '{"operation":"sin","operand1":90,"angleMode":"DEG"}'
```

### 2. Conversión de Unidades (5 km a metros):
```bash
curl -X POST http://localhost:8081/api/calculatorapi/convert \
  -H "Content-Type: application/json" \
  -d '{"category":"length","value":5,"fromUnit":"km","toUnit":"m"}'
```

---

## ☁️ Despliegue en Azure App Service

### Opción 1: Publicación de Contenedores a Azure Container Registry (ACR)
```bash
# Subir Backend a ACR
docker tag scientific-calculator-backend:latest <tu-registro>.azurecr.io/calculator-backend:latest
docker push <tu-registro>.azurecr.io/calculator-backend:latest

# Subir Frontend a ACR
docker tag scientific-calculator-frontend:latest <tu-registro>.azurecr.io/calculator-frontend:latest
docker push <tu-registro>.azurecr.io/calculator-frontend:latest
```

### Opción 2: Publicación con dotnet CLI (Sin Docker)
```bash
dotnet publish -c Release
```
