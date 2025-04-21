# Grupo 01 de Diseño y Aplicaciones de Servicios para Internet

# 🍻 Sistema de Gestión de Bar - Proyecto Académico

<p align="center">
  <a href="https://dotnet.microsoft.com/">
    <img src="https://img.shields.io/badge/.NET_Core-8.0-blue?logo=dotnet" alt=".NET Core">
  </a>
  <a href="https://learn.microsoft.com/en-us/ef/core/">
    <img src="https://img.shields.io/badge/Entity_Framework-Core-green?logo=entity-framework" alt="Entity Framework">
  </a>
  <a href="https://www.microsoft.com/en-us/sql-server/">
    <img src="https://img.shields.io/badge/SQL_Server-2019-red?logo=microsoftsqlserver" alt="SQL Server">
  </a>
  <a href="https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor">
    <img src="https://img.shields.io/badge/Blazor_WebAssembly-client--side-purple?logo=blazor" alt="Blazor WebAssembly">
  </a>
  <a href="https://developer.mozilla.org/en-US/docs/Web/HTML">
    <img src="https://img.shields.io/badge/HTML5-e34f26?logo=html5&logoColor=white" alt="HTML">
  </a>
  <a href="https://developer.mozilla.org/en-US/docs/Web/CSS">
    <img src="https://img.shields.io/badge/CSS3-1572b6?logo=css3&logoColor=white" alt="CSS">
  </a>
  <a href="https://getbootstrap.com/">
    <img src="https://img.shields.io/badge/Bootstrap-7952b3?logo=bootstrap&logoColor=white" alt="Bootstrap">
  </a>
  <a href="https://sweetalert2.github.io/">
    <img src="https://img.shields.io/badge/SweetAlert2-ff69b4?logo=sweetalert&logoColor=white" alt="SweetAlert2">
  </a>
</p>

---

## 👥 Equipo

## 👥 Equipo

| [![Stalin García](https://github.com/stalin21r.png?size=100)](https://github.com/stalin21r) | [![Miguel Pastuña](https://github.com/M147D.png?size=100)](https://github.com/M147D) | [![Dennise Pérez](https://github.com/dennisperezEPN.png?size=100)](https://github.com/dennisperezEPN) | [![Patricio Flor](https://github.com/Aldxir.png?size=100)](https://github.com/Aldxir) |
|:-:|:-:|:-:|:-:|
| **Stalin García** | **Miguel Pastuña** | **Dennise Pérez** | **Patricio Flor** |


---
## 📘 Descripción

Este repositorio contiene el desarrollo del sistema de gestión para un bar, elaborado como parte de la asignatura **Diseño de Aplicaciones y Servicios de Internet**.

El proyecto se desarrolla utilizando **ASP.NET Core Web API como backend** y **Blazor WebAssembly como frontend**, bajo la metodología ágil **Scrum**, con entregables funcionales semanales y una evolución progresiva a lo largo de **16 semanas**.

---

## 🎯 Objetivo del Sistema

Diseñar e implementar una aplicación web que permita:

- 🍹 Registrar productos (bebidas y alimentos)  
- 🧾 Gestionar ventas, pedidos y cuentas por mesa  
- 📦 Controlar el inventario de insumos  
- 📊 Generar reportes de ventas  
- 👤 Administrar roles de usuarios (mesero, administrador, etc.)

---

## 🛠️ Tecnologías Utilizadas

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- ASP.NET Core con Blazor (WebAssembly)
- ASP.NET Core Web API
- SQL Server
- Entity Framework Core
- Visual Studio 2022 o superior
- Postman
- Git y GitHub

---

## 👥 Equipo Scrum

| Nombre              | Rol Scrum       |
|---------------------|-----------------|
| Ing. David Mejía    | Product Owner   |
| Ing. Ricardo Mena   | Scrum Master    |
| Stalin Garcia       | Development Team|
| Miguel Pastuña      | Development Team|
| Dennise Perez       | Development Team|
| Patricio Flor       | Development Team|

---

## 📌 Backlog del Producto

Accede al tablero ágil donde organizamos nuestras **User Stories**:

🔗 [Tablero en Trello - Sistema de Gestión de Bar](https://trello.com/b/WZy7aK5m/sistema-de-gestion-de-bar)

---

## Estructura del Proyecto
    📦
    ├── Backend → API y lógica del servidor (.NET 8)
    │ ├── Controllers → Controladores de la API
    │ ├── Data
    │ │ └── Models → Modelos de la base de datos (EF Core)
    │ ├── Migrations → Migraciones de la base de datos
    │ └── Properties → Configuraciones del proyecto

    ├── Frontend → Interfaz de usuario (Blazor)
    │ ├── Pages → Páginas y componentes principales
    │ ├── Data
    │ │ └── Models → Modelos compartidos
    │ ├── Services → Lógica de servicios (HTTP, etc.)
    │ ├── Layout → Diseños globales
    │ └── wwwroot → Recursos estáticos
    │ └── css → Hojas de estilo (Bootstrap, FontAwesome)


> 🔹 **Backend**: Utiliza ASP.NET Core Web API para gestionar las rutas y la lógica del servidor.  
> 🔹 **Frontend**: Utiliza Blazor WebAssembly para crear la interfaz de usuario y las interacciones con el backend.

---

## 🚀 Cómo Ejecutar el Proyecto

Sigue los siguientes pasos para ejecutar el proyecto en tu entorno local:

1. **Clona el repositorio:**

   Abre una terminal y clona el repositorio con el siguiente comando:

   ```bash
   git clone https://github.com/stalin21r/DASI_GR01.git
   ```

2. **Abre el proyecto en Visual Studio:**

   Navega a la carpeta del proyecto y abre el archivo `DASI_2025A.sln` con Visual Studio.

3. **Crear el archivo `appsettings.json`:**

   En la raíz de la carpeta **Backend**, crea un archivo llamado `appsettings.json` con la siguiente estructura:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server={el nombre del server};Database=gestion_bar;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
     },
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "AllowedHosts": "*"
   }

4. **Restaurar los paquetes NuGet:**

   Abre la consola integrada de Visual Studio (Terminal) y ejecuta el siguiente comando para restaurar los paquetes:

   ```bash
   dotnet restore DASI_2025A.sln
   ```

5. **Actualizar la base de datos:**

   Abre la consola de administración de paquetes NuGet, luego navega al directorio del **Backend** con el siguiente comando:

   ```bash
   cd Backend
   ```
   Asegúrate de que el contexto esté seleccionado en el proyecto Backend y ejecuta el siguiente comando para aplicar las migraciones de la base de datos:

   ```bash
   PM> Update-Database
   ```

6. **Configurar los proyectos de inicio:**

   Para ejecutar tanto el **Frontend** como el **Backend** al mismo tiempo, sigue estos pasos:

   - Haz clic en el botón de **Ejecutar** en Visual Studio.
   - Despliega las opciones y selecciona **Configurar proyectos de inicio**.
   - En la ventana que aparece, selecciona la opción **Múltiples proyectos** y configura ambos (Frontend y Backend) para que se inicien al mismo tiempo.

7. **Compilar la solución:**

   Una vez configurado, compila la solución para asegurarte de que todos los proyectos se construyan correctamente.

8. **Ejecutar el proyecto:**

   Finalmente, ejecuta el proyecto. Se abrirá en tu navegador en **localhost**, y podrás acceder a la aplicación.

---

### 📅 Semana Sprint / Entregable Estado

| Semana | Sprint / Entregable                | Estado      |
|--------|------------------------------------|-------------|
| 1      | Configuración de entorno y backlog | ✅ Terminado |
| 2      | Diseño general del sistema         | 🕐 Planeación |
| 3      | Registro y gestión de productos    | 🕐 Planeación |
| 4      | Módulo de pedidos                  | 🕐 Planeación |

---

### 📝 Notas Adicionales

- Este proyecto es con fines académicos y está desarrollado como parte de la asignatura **Diseño de Aplicaciones y Servicios de Internet**.
- El desarrollo sigue una metodología **ágil (Scrum)** con entregas incrementales y feedback constante.
- Para **reportar errores** o sugerir **mejoras**, por favor utiliza la sección de **[Issues](https://github.com/stalin21r/DASI_GR01/issues)** en este repositorio. 

### 🚀 Instrucciones para Issues

1. **Crear un Issue**: Si encuentras un problema o tienes una sugerencia de mejora, abre un nuevo **Issue** en el repositorio.
2. **Proveer detalles**: Asegúrate de proporcionar la mayor cantidad de información posible:
    - Descripción del problema o mejora.
    - Pasos para reproducir el error (si aplica).
    - Capturas de pantalla o ejemplos de código si son relevantes.
3. **Asignación**: Los Issues se asignarán a los miembros del equipo según la prioridad. Los Issues que no están asignados se consideran pendientes.
4. **Comentarios y actualizaciones**: Si estás trabajando en un Issue, actualiza el estado en los comentarios para mantener a todos al tanto de los avances.

### 📋 Seguimiento y Prioridades

- **Baja prioridad**: Mejoras menores o características adicionales que pueden implementarse en fases posteriores.
- **Alta prioridad**: Errores críticos que afectan la funcionalidad del sistema.

Recuerda que para contribuir, se recomienda trabajar en ramas separadas y abrir **pull requests** una vez hayas completado el trabajo relacionado con un Issue.
