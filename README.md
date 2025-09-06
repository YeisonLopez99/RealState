# 🏡 RealEstate API

API RESTful desarrollada en **.NET 8** con arquitectura limpia (**Domain, Application, Infrastructure, Api**).  
Gestión de propiedades inmobiliarias.

---

## 📌 Arquitectura
Solution
├── Api -> Endpoints, Swagger, Middlewares
├── Application -> Casos de uso, DTOs, Handlers (CQRS + MediatR)
├── Domain -> Entidades y lógica de negocio
├── Infrastructure -> EF Core, Repositorios, JWT, Persistencia
└── Tests -> Pruebas unitarias y de integración


- **Domain**: Solo contiene reglas de negocio (Entidades + Value Objects).  
- **Application**: Orquesta casos de uso, valida requests, usa repositorios abstraídos.  
- **Infrastructure**: Implementa persistencia (EF Core), seguridad (JWT), logging, etc.  
- **Api**: Expone controladores HTTP y documentación con Swagger.  

---

## 🚀 Cómo correr el proyecto

1. Clonar el repo:
   ```bash
   git clone https://github.com/tu-repo/realestate-api.git
   cd realestate-api
   
2. Configurar la base de datos en appsettings.json:

   "ConnectionStrings": {
  	"DefaultConnection": "Server=localhost;Database=RealEstateDb;Trusted_Connection=True;TrustServerCertificate=True"
	}
3. Ejecutar migraciones:

    dotnet ef database update --project Infrastructure --startup-project Api

4. Lanzar la API:
    dotnet run --project Api 

5. Acceder a Swagger:
    http://localhost:5000/swagger  


##🔑 Autenticación

Se utiliza JWT (Bearer Token).

Endpoint para login:
POST /api/v1/auth/login
{
  "username": "owner@example.com",
  "password": "Pa$$w0rd"
}
Copiar el token y pegarlo en Swagger con Authorize.


📚 Endpoints principales

POST /api/v1/properties → Crear propiedad.

GET /api/v1/properties → Listar propiedades (con imágenes).

PATCH /api/v1/properties/{id}/change-price → Cambiar precio de propiedad.

POST /api/v1/properties/{id}/images → Subir imagen a una propiedad.

🧪 Pruebas

Ejecutar pruebas unitarias:

dotnet test