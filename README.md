# InvestBetterPlan
Proyecto RestApi prueba técnica para Better Plan

Para comenzar
------------
1. Poner el proyecto en una carpeta "Projects"
2. Paquetes NuGets que se utilizó:
	2.1 Microsoft.EntityFrameworkcore version 6.0.7
	2.2 Microsoft.EntityFrameworkcore.Tools version 6.0.7
	2.3 Microsoft.EntityFrameworkcore.Design version 6.0.7
	2.4 Npgsql.EntityFrameworkcore.PostgreSQL version 6.0.7

Estructura del Proyecto
-----------------------
El API rest que he construido se basa en ASP.NET MVC  
1. Models: Donde están las clases de mis entidades, mis constantes y mis DTO’s. de PostgreSql.
2. Repository: En está carpeta está el repositorio donde se encuentran las clases e interfaces que administra todo lo relacionado con interacción de la base de datos de PostgreSQL mediante LINQ.
3. Controllers: La clase BetterPlanAPIController que contiene los métodos de los 4  endpoints.

Probar API
----------
He publicado mi API en el siguiente link:
http://solucionbetterplan.somee.com

1. Para la primera tarea "Traer usuario" mi path es: 
https:// solucionbetterplan.somee.com/api/users/{id}, donde ‘id’ es el userID.

2. Para la segunda tarea "Traer resumen usuario actual" mi path es: 
https:// solucionbetterplan.somee.com/api/users/{id}/summary, donde ‘id’ es el userId.
3. Para la cuarta tarea "Traer metas de un usuario" mi path es: 
https:// solucionbetterplan.somee.com/api/users/{id}/goals, donde ‘id’ es el userId.

4. Para la quinta tarea "Traer detalles de una meta" mi path: 
https:// solucionbetterplan.somee.com /api/users/{id}/goals/{goalid}, donde ‘id’ es el userId.

