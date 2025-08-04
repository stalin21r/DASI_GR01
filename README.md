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
    <img src="https://img.shields.io/badge/SQL_Server-2022-red?logo=microsoftsqlserver" alt="SQL Server">
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
  <a href="https://tailwindcss.com/">
    <img src="https://img.shields.io/badge/TailwindCSS-06B6D4?logo=tailwindcss&logoColor=white" alt="TailwindCSS">
  </a>
  <a href="https://sweetalert2.github.io/">
    <img src="https://img.shields.io/badge/SweetAlert2-ff69b4?logo=sweetalert&logoColor=white" alt="SweetAlert2">
  </a>
</p>

---

## 👥 Colaboradores
<div align="center">
  <table>
    <tr>
      <td align="center">
        <a href="https://github.com/stalin21r">
          <img src="https://github.com/stalin21r.png" width="80" alt="Stalin Garcia"/><br/>
          <sub><b>Stalin Garcia</b></sub>
        </a>
      </td>
      <td align="center">
        <a href="https://github.com/M147D">
          <img src="https://github.com/M147D.png" width="80" alt="Miguel Pastuña"/><br/>
          <sub><b>Miguel Pastuña</b></sub>
        </a>
      </td>
      <td align="center">
        <a href="https://github.com/dennisperezEPN">
          <img src="https://github.com/dennisperezEPN.png" width="80" alt="Dennise Perez"/><br/>
          <sub><b>Dennis Perez</b></sub>
        </a>
      </td>
      <td align="center">
        <a href="https://github.com/Aldxir">
          <img src="https://github.com/Aldxir.png" width="80" alt="Patricio Flor"/><br/>
          <sub><b>Patricio Flor</b></sub>
        </a>
      </td>
    </tr>
  </table>
</div>



---
## 📘 Descripción

Este repositorio contiene el desarrollo del sistema de gestión para un bar, elaborado como parte de la asignatura **Diseño de Aplicaciones y Servicios de Internet**.

El proyecto se desarrolla utilizando **ASP.NET Core Web API como backend** y **Blazor WebAssembly como frontend**, bajo la metodología ágil **Scrum**, con entregables funcionales semanales y una evolución progresiva a lo largo de **16 semanas**.

---

## 🎯 Objetivo del Sistema

Diseñar e implementar una aplicación web que permita:

-🍹 Registrar y administrar productos (bebidas y alimentos)
-🧾 Gestionar ventas, pedidos y control de despacho
-💰 Administrar saldos y recargas de usuarios (efectivo o transferencia)
-📄 Procesar solicitudes de recarga con comprobantes adjuntos
-📊 Generar reportes detallados de ventas, recargas y movimientos
-👤 Administrar usuarios con roles jerárquicos (scout, jefe, admin, superadmin)
-🔐 Garantizar seguridad en frontend y backend mediante autenticación, autorización y manejo de rutas
-🧭 Navegación segura y controlada según el rol de usuario
-🔍 Aplicar filtros eficientes en módulos de usuarios, órdenes y recargas
-📧 Incluir funcionalidades de activación de cuenta y recuperación de contraseña mediante correo electrónico
-📄 Mostrar páginas informativas de Términos y Condiciones y Políticas de Privacidad



---

## 🛠️ Tecnologías Utilizadas

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- ASP.NET Core con Blazor (WebAssembly)
- ASP.NET Core Web API
- SQL Server
- Entity Framework Core
- Visual Studio 2022 o superior
- Tailwind 4
- Postman
- Git y GitHub

---

## 👥 Equipo Scrum

| Nombre              | Rol Scrum       |
|---------------------|-----------------|
| David Mejía         | Product Owner   |
| Ricardo Mena        | Scrum Master    |
| Stalin Garcia       | Development Team|
| Miguel Pastuña      | Development Team|
| Dennise Perez       | Development Team|
| Patricio Flor       | Development Team|

---

## 📌 Backlog del Producto

Accede al tablero ágil donde organizamos nuestras **User Stories**:

🔗 [Tablero en Notion - Sistema de Gestión de Bar](https://discreet-denim-afc.notion.site/1e26e2532fa98087b2a3f731570748c9?v=1e26e2532fa9812689a6000c1a781e54)

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
    "AllowedHosts": "*",
    "Superadmin": {
      "Email": "aquiles.superadmin@epn.edu.ec",
      "Password": "@Quil3s123"
    },
    "Jwt": {
      "Key": "3FA94C2D7E8B9D4F123456789ABCDEF0",
      "Issuer": "Scouts#18SSCCRumipamba",
      "Audience": "Scouts",
      "ExpireMinutes": 180,
      "RefreshTokenExpireDays": 7
    },
    "Imgur": {
      "ImgutToken": "<Imgur_Token>",
      "ImgurRefreshToken": "<Imgur_Refresh_Token>",
      "ImgurClientId": "<Imgur_Client_Id>"
    },
    "Smtp": {
      "Host": "smtp.gmail.com",
      "Port": "587",
      "User": "<Correo_Gmail>",
      "Pass": "<Contraseña_Aplicacion>",
      "From": "<Correo_Gmail>"
    },
    "App": {
      "UrlBase": "https://localhost:7206/"
    }
  }


1. **Instalar Tailwind cli (solo desarrollo)** 
  
   Abre una consola (power shell o bash), dirigete a la carpeta Frontend dentro de  DASI_2025A, ejecutar el comando:
  
   ```bash
   npm install 
   ```

   Nota: solo se debe instalar una vez.
   
   Al terminar la instalacion, ejecutar el siguiente comando:

   ```bash
   npx @tailwindcss/cli -i ./styles/input.css -o ./wwwroot/css/tailwind.css --watch
   ```

   Esto ejecutara la compilacion de tailwind en tiempo real, cabe aclarar que este proceso solo se debe hacer para el modo desarrollo.
   
   En modo produccion no es necesario hacer este paso puesto que ya se tendra el css de tailwind compilado.

   > **⚠️ Importante:** Recuerda tener instalado Node.js (https://nodejs.org/es)

2. **Restaurar los paquetes NuGet:**

   Herramientas > Administrador de paquetes NuGet > Consola del Administrador de paquetes

   Abre la consola integrada de Visual Studio (Terminal) y ejecuta el siguiente comando para restaurar los paquetes:

   ```bash
   Update-Package -Reinstall
   ```

3. **Actualizar la base de datos:**

   Abre la consola de administración de paquetes NuGet, luego navega al directorio del **Backend** con el siguiente comando:

   ```bash
   cd Backend
   ```
   Asegúrate de que el contexto esté seleccionado en el proyecto Backend y ejecuta el siguiente comando para aplicar las migraciones de la base de datos:

   ```bash
   Update-Database
   ```

4. **Configurar los proyectos de inicio:**

   Para ejecutar tanto el **Frontend** como el **Backend** al mismo tiempo, sigue estos pasos:

   - Haz clic en el botón de **Ejecutar** en Visual Studio.
   - Despliega las opciones y selecciona **Configurar proyectos de inicio**.
   - En la ventana que aparece, selecciona la opción **Múltiples proyectos** y configura ambos (Frontend y Backend) para que se inicien al mismo tiempo.

5. **Compilar la solución:**

   Una vez configurado, compila la solución para asegurarte de que todos los proyectos se construyan correctamente.

6.  **Ejecutar el proyecto:**

   Finalmente, ejecuta el proyecto. Se abrirá en tu navegador en **localhost**, y podrás acceder a la aplicación.

---

### 📅 Semana Sprint / Entregable Estado

| Semana | Sprint / Entregable                           | Estado        |
|--------|-----------------------------------------------|---------------|
| 1      | Configuración de entorno y backlog            | ✅ Terminado  |
| 2      | Diseño general del sistema                    | ✅ Terminado  |
| 3      | Usuarios y Login                              | ✅ Terminado  |
| 4      | Módulo de Productos                           | ✅ Terminado  |
| 5      | Módulo de Órdenes                             | ✅ Terminado  |
| 6      | Módulo de Administrar Recargas                | ✅ Terminado  |
| 7      | Módulo de Solicitudes de Recarga              | ✅ Terminado  |
| 8      | Filtros en Módulo de Usuarios                 | ✅ Terminado  |
| 9      | Filtros en Módulo de Órdenes                  | ✅ Terminado  |
| 10     | Filtros en Módulo de Recargas                 | ✅ Terminado  |
| 11     | Seguridad en Backend (roles, validaciones)    | ✅ Terminado  |
| 12     | Seguridad en Frontend (autenticación, guards) | ✅ Terminado  |
| 13     | Manejo de rutas y navegación segura           | ✅ Terminado  |
| 14     | Recuperación de contraseña vía correo         | ✅ Terminado  |
| 15     | Activación de usuarios vía correo electrónico | ✅ Terminado  |
| 16     | Página de Términos y Condiciones, Página de Políticas de Privacidad            | ✅ Terminado  |
| 17     | Página de Políticas de Privacidad             | ✅ Terminado  |


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
