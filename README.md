# Calendar-Api

El proyecto tiene como objetivo el desarrollo del backend del siguiente proyecto <a href="https://github.com/raudel25/Calendar-App">Calendar-App</a>.

### Ejecutando el Proyecto

El Proyecto se encuentra desarrollado en **.Net 7** y se usó **Mysql** como base de datos junto a **Entity Framework Core**. Para ejecutarlo, debe configurar el scrip de conexiona su base de datos sustituyendo los valores que se encuentran en `appsettings.json`, luego debe ejecutar los siguientes comandos en su terminal.

```
make restore
```

o

```
dotnet restore
```

para actualizar las dependencias,

```
make db
```

o

```
dotnet ef database update --project Calendar-Api
```

para crear y actualizar la base de datos y finalmente

```
make dev
```

o

```
dotnet run --project Calendar-Api
```

para correr la api.
