# NotiGest

## Descripción

Este proyecto es una aplicación basada en .NET Core que utiliza una arquitectura en capas para separar la accesibilidad de la data. Se ha implementado la librería Mapster para la transformación eficiente de datos, Hangfire para la ejecución de tareas en segundo plano (como la inserción de noticias), y Fluent Validation para la validación de endpoints.

Además, la aplicación incluye una integración con Entity Framework (EF) para facilitar la creación y manipulación de la base de datos. Se han utilizado varios paquetes adicionales para mejorar la funcionalidad de la aplicación, incluyendo RestSharp para realizar solicitudes HTTP, NRedisStack para la interacción con Redis, JwtBearer para la autenticación basada en tokens JWT, y AspNetCoreRateLimit para el control de la velocidad de las solicitudes.

## Características Principales

- **Arquitectura en Capas:** La aplicación sigue una arquitectura en capas que separa la lógica de presentación, la lógica de negocios y el acceso a datos. Esto facilita la modularidad y el mantenimiento del código.

- **Mapster:** Se ha utilizado Mapster para facilitar la transformación de datos entre diferentes modelos y DTOs, simplificando así la manipulación de información en la aplicación.

- **Hangfire:** Hangfire se ha incorporado para ejecutar tareas en segundo plano, como la inserción de noticias. Esto mejora la eficiencia y la escalabilidad de la aplicación.

- **Fluent Validation:** Se han utilizado reglas de validación claras y expresivas con Fluent Validation para garantizar que los endpoints cumplan con los requisitos de entrada.

- **Entity Framework (EF):** EF se utiliza para interactuar con la base de datos, proporcionando una forma cómoda y eficiente de gestionar entidades y realizar operaciones CRUD.

- **Paquetes Adicionales:**
  - **RestSharp:** Para realizar solicitudes HTTP de manera sencilla y eficiente.
  - **NRedisStack:** Para la integración con Redis y mejorar el rendimiento de ciertas operaciones.
  - **JwtBearer:** Para la autenticación basada en tokens JWT, proporcionando seguridad a la aplicación.
  - **AspNetCoreRateLimit:** Para controlar la velocidad de las solicitudes y prevenir abusos.

## Configuración
Configurar conexion a bd y tener redis cache y para front configurar URL (Si utiiza el usuario admin(Gustavo@admin.com - Password123456---) puede ver el contenido de /usuarios)


-- No disponible
A continuación, se proporciona información sobre la configuración necesaria para ejecutar la aplicación localmente y para su despliegue, actualmente tiene un action que se encarga de ejecutar el dockerfile y subir la imagen al registry de docker

para su ejecucion local puede obtar por usar el docker-compose con el comando 'docker-compose up -d' y levantar el entorno de pruebas (recuerde crear la network con 'docker network create NotiGest')o ejecutar los proyectos individuales 

el proyecto back esta desarrollado con .net 8 y el del front node 20 y angular 17 y tener en cuenta el uso tanto de redis cache desde un container de docker y las configuraciones en las URL y conexiones a la base de datos del back 

[proyecto front](https://github.com/gustavo1020/NotiGestFront)
1- para este proyecto primero usar el npm install y despues el ng serve

