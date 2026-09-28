# Práctica progresiva de Web API — Programación II

El workspace contiene siete proyectos ASP.NET Core Web API independientes, uno por ejercicio. Cada API mantiene sus datos en memoria: al detener y volver a iniciar el proyecto, los registros se reinician.

## Requisitos

- .NET 8 SDK

## Proyectos

| Carpeta | Ejercicio | Ruta base |
| --- | --- | --- |
| `MiApi` | Centro de objetos perdidos | `/api/objetosperdidos` |
| `MiApi2` | Gestión de estaciones meteorológicas | `/api/estaciones` |
| `MiApi3` | Plataforma de desafíos de programación | `/api/desafios` |
| `MiApi4` | Centro de control de drones | `/api/drones` |
| `MiApi5` | Festival tecnológico | `/api/festival` |
| `MiApi6` | Centro de monitoreo espacial | `/api/objetosespaciales` |
| `MiApi7` | Sistema de expediciones | `/api/expediciones` |

## Ejecutar una API

Desde la raíz del workspace, elegí el proyecto que quieras iniciar:

```bash
dotnet run --project MiApi/MiApi.csproj
dotnet run --project MiApi2/MiApi2.csproj
dotnet run --project MiApi3/MiApi3.csproj
dotnet run --project MiApi4/MiApi4.csproj
dotnet run --project MiApi5/MiApi5.csproj
dotnet run --project MiApi6/MiApi6.csproj
dotnet run --project MiApi7/MiApi7.csproj
```

Al iniciar, la consola muestra la dirección local. Abrí `/swagger` en esa dirección para explorar y probar los endpoints. Cada carpeta también incluye un archivo `.http` con solicitudes de ejemplo.

## Endpoints

Los segmentos dinámicos se escriben entre paréntesis en la URL. Por ejemplo, el ID `3` se envía como `(3)`. Las llaves dentro de las anotaciones de C# indican a ASP.NET Core qué parte de la URL debe enlazar con un parámetro.

### MiApi — Objetos perdidos

| Método | Ruta | Acción |
| --- | --- | --- |
| `GET` | `/api/objetosperdidos` | Listar, del más reciente al más antiguo |
| `GET` | `/api/objetosperdidos/(id)` | Buscar por ID |
| `POST` | `/api/objetosperdidos` | Registrar objeto |
| `PUT` | `/api/objetosperdidos/(id)` | Modificar objeto |
| `DELETE` | `/api/objetosperdidos/(id)` | Eliminar objeto |
| `GET` | `/api/objetosperdidos/buscar?descripcion=...` | Buscar por descripción |
| `GET` | `/api/objetosperdidos/categoria/(categoria)` | Filtrar por categoría |
| `GET` | `/api/objetosperdidos/no-reclamados` | Listar objetos sin reclamar |
| `GET` | `/api/objetosperdidos/posteriores?fecha=...` | Filtrar por fecha |
| `GET` | `/api/objetosperdidos/cantidad` | Contar objetos |
| `PATCH` | `/api/objetosperdidos/(id)/reclamar` | Registrar quién retiró el objeto |

### MiApi2 — Estaciones meteorológicas

| Método | Ruta | Acción |
| --- | --- | --- |
| `POST` / `GET` | `/api/estaciones` | Crear y listar estaciones |
| `GET` | `/api/estaciones/localidad/(localidad)` | Buscar por localidad |
| `POST` | `/api/estaciones/mediciones` | Registrar medición |
| `GET` | `/api/estaciones/mediciones` | Listar mediciones |
| `GET` | `/api/estaciones/(id)/mediciones` | Mediciones de una estación |
| `GET` | `/api/estaciones/temperaturas-superiores/(valor)` | Filtrar por temperatura mínima |
| `GET` | `/api/estaciones/temperaturas-ordenadas` | Ordenar por temperatura |
| `GET` | `/api/estaciones/estadisticas` | Promedio, máxima y mínima |
| `GET` | `/api/estaciones/cantidad-por-estacion` | Contar mediciones por estación |

### MiApi3 — Desafíos de programación

| Método | Ruta | Acción |
| --- | --- | --- |
| `POST` | `/api/desafios/participantes` | Registrar participante |
| `POST` | `/api/desafios` | Crear desafío |
| `POST` | `/api/desafios/resoluciones` | Registrar resolución |
| `GET` | `/api/desafios/activos` | Listar desafíos activos |
| `GET` | `/api/desafios/dificultad/(valor)` | Filtrar por dificultad |
| `GET` | `/api/desafios/participantes/(id)/resoluciones` | Resoluciones de un participante |
| `GET` | `/api/desafios/participantes/superaron/(puntaje)` | Participantes sobre un puntaje |
| `GET` | `/api/desafios/(id)/promedio` | Promedio de puntaje del desafío |
| `GET` | `/api/desafios/ranking` | Puntaje acumulado por participante |
| `GET` | `/api/desafios/mejor-participante` | Participante con mayor puntaje |
| `GET` | `/api/desafios/participantes/(id)/pendientes` | Desafíos activos aún no resueltos |

### MiApi4 — Drones

| Método | Ruta | Acción |
| --- | --- | --- |
| `GET` / `POST` | `/api/drones` | Listar y registrar drones |
| `GET` / `PUT` / `DELETE` | `/api/drones/(id)` | Consultar, modificar o eliminar drone |
| `GET` | `/api/drones/disponibles` | Drones disponibles |
| `GET` | `/api/drones/bateria-inferior/(porcentaje)` | Filtrar por batería |
| `GET` | `/api/drones/modelo/(modelo)` | Buscar por modelo |
| `POST` | `/api/drones/misiones` | Asignar misión |
| `PATCH` | `/api/drones/misiones/(id)/finalizar` | Finalizar misión |
| `GET` | `/api/drones/(id)/misiones` | Misiones de un drone |
| `GET` | `/api/drones/distancias` | Distancia recorrida por drone |
| `GET` | `/api/drones/mayor-distancia` | Drone con mayor distancia |
| `GET` | `/api/drones/promedio-bateria` | Promedio de batería disponible |
| `GET` | `/api/drones/misiones-pendientes` | Misiones pendientes por fecha |

### MiApi5 — Festival tecnológico

| Método | Ruta | Acción |
| --- | --- | --- |
| `POST` | `/api/festival/actividades` | Crear actividad |
| `POST` | `/api/festival/participantes` | Registrar participante |
| `POST` | `/api/festival/reservas` | Inscribir participante |
| `DELETE` | `/api/festival/reservas/(id)` | Cancelar inscripción |
| `GET` | `/api/festival/actividades` | Actividades disponibles |
| `GET` | `/api/festival/actividades/(id)/participantes` | Participantes de una actividad |
| `GET` | `/api/festival/participantes/(id)/actividades` | Actividades de un participante |
| `GET` | `/api/festival/actividades/buscar?nombre=...` | Buscar actividad por nombre |
| `GET` | `/api/festival/actividades/tipo/(tipo)` | Filtrar por tipo |
| `GET` | `/api/festival/actividades/completas` | Actividades completas |
| `GET` | `/api/festival/actividades/ranking` | Inscriptos y ocupación |
| `GET` | `/api/festival/actividades/mas-popular` | Actividad con más participantes |

### MiApi6 — Monitoreo espacial

| Método | Ruta | Acción |
| --- | --- | --- |
| `POST` | `/api/objetosespaciales` | Registrar objeto espacial |
| `POST` | `/api/objetosespaciales/observaciones` | Registrar observación |
| `GET` | `/api/objetosespaciales/(id)/observaciones` | Observaciones de un objeto |
| `GET` | `/api/objetosespaciales/buscar?nombre=...` | Buscar por nombre |
| `GET` | `/api/objetosespaciales/tipo/(tipo)` | Filtrar por tipo |
| `GET` | `/api/objetosespaciales/riesgo/(nivel)` | Filtrar por riesgo |
| `GET` | `/api/objetosespaciales/cercanos/(distancia)` | Filtrar por distancia |
| `GET` | `/api/objetosespaciales/promedio-velocidad` | Velocidad promedio observada |
| `GET` | `/api/objetosespaciales/mas-observado` | Objeto con más observaciones |
| `GET` | `/api/objetosespaciales/sin-observaciones` | Objetos sin observaciones |
| `GET` | `/api/objetosespaciales/estadisticas` | Estadísticas generales |
| `GET` | `/api/objetosespaciales/alertas` | Alertas (acepta `riesgoMinimo` y `distanciaMaxima`) |

### MiApi7 — Expediciones

| Método | Ruta | Acción |
| --- | --- | --- |
| `GET` / `POST` | `/api/expediciones` | Listar y crear expediciones |
| `GET` / `PUT` / `DELETE` | `/api/expediciones/(id)` | Consultar, modificar o eliminar expedición |
| `POST` | `/api/expediciones/exploradores` | Registrar explorador |
| `POST` | `/api/expediciones/participaciones` | Asignar explorador |
| `GET` | `/api/expediciones/(id)/integrantes` | Integrantes de una expedición |
| `GET` | `/api/expediciones/exploradores/(id)/expediciones` | Expediciones de un explorador |
| `GET` | `/api/expediciones/exploradores/especialidad/(valor)` | Buscar especialidad |
| `GET` | `/api/expediciones/exploradores/disponibles` | Exploradores disponibles |
| `GET` | `/api/expediciones/activas` | Expediciones activas |
| `GET` | `/api/expediciones/futuras` | Expediciones futuras |
| `GET` | `/api/expediciones/completas` | Expediciones completas |
| `GET` | `/api/expediciones/ocupacion` | Cantidad y porcentaje de ocupación |
| `GET` | `/api/expediciones/exploradores/ranking` | Exploradores con más participaciones |
| `GET` | `/api/expediciones/especialidades/ranking` | Especialidades más utilizadas |
