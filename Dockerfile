###### Stage 1 - Build and publish #################
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS builder
WORKDIR /app

#### Copy csprojs only to allow for layer matching
# Core projects
COPY src/Host/Host.csproj src/Host/
COPY src/RuntimeLoggingChanges/RuntimeLoggingChanges.csproj src/RuntimeLoggingChanges/
COPY src/HttpApi/HttpApi.csproj src/HttpApi/

#### Restore the solution
RUN dotnet restore src/Host

#### Copy remaining project files
COPY src src
COPY quality quality

#### Build the release artifacts
RUN dotnet publish \
  --configuration Release \
  --output out \
  src/Host /WarnAsError

###### Stage 2 - Publish ###########################
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

# Copy release build artifacts
COPY --from=builder /app/out .

ENV ASPNETCORE_URLS=http://+:5002
EXPOSE 5002

ENTRYPOINT ["dotnet", "Host.dll"]