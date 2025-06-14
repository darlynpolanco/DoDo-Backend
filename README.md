# 🦤 DoDo-Backend

¡Bienvenido/a a **DoDo-Backend**!  
API RESTful en C# para una gestión de tareas fácil, rápida y divertida.

---

## 🚀 Características

- ✅ CRUD de tareas y usuarios
- 🔐 Autenticación JWT
- 🔄 Endpoints bien organizados
- 🐞 Manejo de errores centralizado
- 📦 Estructura modular y limpia

---

## 🛠️ Instalación

1. **Clona el repositorio**
   ```bash
   git clone https://github.com/darlynpolanco/DoDo-Backend.git
   cd DoDo-Backend
   ```
2. **Restaura dependencias**
   ```bash
   dotnet restore
   ```

---

## ⚙️ Configuración Rápida

1. Crea un archivo `appsettings.json` (o copia el ejemplo si existe):

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=DoDoDb;User Id=sa;Password=your_password;"
     },
     "Jwt": {
       "Key": "your_jwt_secret_key",
       "Issuer": "DoDoApi"
     }
   }
   ```

---

## ▶️ Ejecución

Arranca el servidor localmente con:

```bash
dotnet run
```

La API estará disponible en:  
🌐 `https://localhost:5001`  
🌐 `http://localhost:5000`

---

## 📚 Endpoints Principales

| Método  | Ruta               | Descripción               |
|---------|--------------------|---------------------------|
| GET     | `/api/tasks`       | Listar tareas             |
| POST    | `/api/tasks`       | Crear nueva tarea         |
| PUT     | `/api/tasks/{id}`  | Actualizar tarea          |
| DELETE  | `/api/tasks/{id}`  | Eliminar tarea            |
| POST    | `/api/auth/login`  | Iniciar sesión usuario    |

> 💡 **Nota:** Algunos endpoints requieren autenticación JWT.

---

## 📦 Estructura del Proyecto

```
DoDo-Backend/
├── Controllers/
│   └── TasksController.cs
├── Models/
│   └── TaskModel.cs
├── Services/
│   └── TaskService.cs
├── Data/
│   └── ApplicationDbContext.cs
├── appsettings.json
└── ...
```

---

## 🧑‍💻 Ejemplos de uso de Endpoints

### 🔑 Login de usuario

```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "tu_usuario",
  "password": "tu_contraseña"
}
```

**Respuesta exitosa:**
```json
{
  "token": "jwt_aqui"
}
```

---

### 📝 Crear una nueva tarea

```http
POST /api/tasks
Authorization: Bearer TU_TOKEN_JWT
Content-Type: application/json

{
  "title": "Comprar leche",
  "description": "Recuerda comprar leche descremada"
}
```

---

### 📋 Listar todas las tareas

```http
GET /api/tasks
Authorization: Bearer TU_TOKEN_JWT
```

**Respuesta:**
```json
[
  {
    "id": 1,
    "title": "Comprar leche",
    "description": "Recuerda comprar leche descremada",
    "isCompleted": false
  }
]
```

---

### 🗑️ Eliminar una tarea

```http
DELETE /api/tasks/1
Authorization: Bearer TU_TOKEN_JWT
```

---

## 🙌 Contribuciones

¿Quieres mejorar DoDo-Backend?  
¡Los PRs, issues y sugerencias son bienvenidos!  
Haz fork al repo y envía tu propuesta 🚀

---

## 📄 Licencia

MIT
