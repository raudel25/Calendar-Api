.PHONY: dev
dev:
	dotnet run --project Calendar-Api

.PHONY: restore
restore:
	dotnet restore

.PHONY: db
db:
	dotnet dotnet ef database update --project Calendar-Api

.PHONY: build
build:
	dotnet build